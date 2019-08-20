using MongoDB.Driver;
using MongodbAccess.Services;
using RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess.Implementations
{
    public class MongodbSaveRepository<T> : MongodbRepository<T>, ISaveRepository<T>
    {
        public MongodbSaveRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public async Task SaveAsync(T entity)
        {
            await this.MongoCollection.InsertOneAsync(entity);
        }

        public async Task SaveAsync(IList<T> entities)
        {
            await this.MongoCollection.InsertManyAsync(entities);
        }

        public async Task UpdateAsync(Expression<Func<T, bool>> expression, T entity)
        {
            await this.MongoCollection.ReplaceOneAsync(expression, entity);
        }

        public async Task UpdateAsync(Expression<Func<T, bool>> expression, IList<T> entities)
        {
            Task[] tasks = new Task[entities.Count];

            for (int i = 0; i < entities.Count; i++)
            {
                T entity = entities[i];
                tasks[i] = this.UpdateAsync(expression, entity);
            }

            await Task.WhenAll(tasks);
        }
    }
}
