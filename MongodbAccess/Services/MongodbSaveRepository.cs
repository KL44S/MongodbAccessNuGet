using MongoDB.Driver;
using MongodbAccess.Model;
using MongodbAccess.Services;
using RepositoryAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess.Implementations
{
    internal class DoUpdateManyResult
    {
        public IList<string> SuccedeedIds { get; set; }
        public IList<string> TransientFailedIds { get; set; }
        public IList<string> PermanentFailedIds { get; set; }
    }

    public class MongodbSaveRepository<T> : MongodbRepository<T>, IMongodbSaveRepository<T> where T : IdObject
    {
        private const int MaxRetriesNumber = 3;

        public MongodbSaveRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public async Task SaveAsync(T entity)
        {
            await this._mongoCollection.InsertOneAsync(entity);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var result = await this._mongoCollection.ReplaceOneAsync(
                x => x.ObjectId == entity.ObjectId,
                entity);

            return result.IsAcknowledged && result.MatchedCount == 1 && result.ModifiedCount == 1;
        }

        public async Task<UpdateManyResult> UpdateAsync(IList<T> entities)
        {
            int retriesNumber = 0;
            IList<string> succedeedIds = new List<string>();
            IList<string> transientFailedIds = new List<string>();
            IList<T> entitiesToProcess = entities;
            IList<string> permanentFailedIds = new List<string>();

            while (entities.Count > 0 && retriesNumber < MaxRetriesNumber)
            {
                DoUpdateManyResult doUpdateManyResult = await this.DoUpdateAsync(entitiesToProcess);

                succedeedIds = succedeedIds.Union(doUpdateManyResult.SuccedeedIds).ToList();
                transientFailedIds = doUpdateManyResult.TransientFailedIds;
                permanentFailedIds = permanentFailedIds.Concat(doUpdateManyResult.PermanentFailedIds).ToList();

                entitiesToProcess = entities.Where(x => transientFailedIds.Contains(x.ObjectId)).ToList();
                retriesNumber++;
            }

            permanentFailedIds = permanentFailedIds.Union(transientFailedIds).ToList();

            UpdateManyResult updateManyResult = new UpdateManyResult()
            {
               SuccessfulIds = succedeedIds,
               FailedIds = permanentFailedIds
            };

            return updateManyResult;
        }

        public async Task SaveAsync(IList<T> entities)
        {
            await this._mongoCollection.InsertManyAsync(entities);
        }

        public async Task UpdateAsync(Expression<Func<T, bool>> expression, T entity)
        {
            await this._mongoCollection.ReplaceOneAsync(expression, entity);
        }

        public async Task<bool> UpdateMany(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition)
        {
            var result = await this._mongoCollection.UpdateManyAsync(expression, updateDefinition);

            return (result.IsAcknowledged && result.MatchedCount > 0 && result.ModifiedCount > 0);
        }

        private async Task<DoUpdateManyResult> DoUpdateAsync(IList<T> entities)
        {
            IList<string> succedeedIds = new List<string>();
            IList<string> transientFailedIds = new List<string>();
            IList<string> permanentFailedIds = new List<string>();
            IList<Task> updateTasks = new List<Task>();

            foreach (T entity in entities)
            {
                var updateTask = this._mongoCollection.ReplaceOneAsync(
                    x => x.ObjectId == entity.ObjectId,
                    entity);

                var task = updateTask.ContinueWith(x =>
                {
                    var result = x.Result;

                    if (result.IsAcknowledged)
                    {
                        if (result.MatchedCount == 1)
                        {
                            if (result.ModifiedCount == 1)
                            {
                                succedeedIds.Add(entity.ObjectId);
                            }
                            else
                            {
                                transientFailedIds.Add(entity.ObjectId);
                            }
                        }
                        else
                        {
                            permanentFailedIds.Add(entity.ObjectId);
                        }
                    }
                    else
                    {
                        transientFailedIds.Add(entity.ObjectId);
                    }
                });

                updateTasks.Add(task);
            }

            await Task.WhenAll(updateTasks);

            var updateManyResult = new DoUpdateManyResult()
            {
                 PermanentFailedIds = permanentFailedIds,
                 SuccedeedIds = succedeedIds,
                 TransientFailedIds = transientFailedIds
            };

            return updateManyResult;
        }
    }
}