using MongoDB.Driver;
using MongodbAccess.Model;
using MongodbAccess.Services;
using MongodbAccess.Utils;
using RepositoryAbstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MongodbAccess.Implementations
{
    public class MongodbSaveRepository<T> : MongodbRepository<T>, IMongodbSaveRepository<T>
    {

        public MongodbSaveRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public async Task<SaveResult> UpdateManyAsync(
            Expression<Func<T, bool>> expression, 
            UpdateDefinition<T> updateDefinition,
            CancellationToken cancellationToken = default)
        {
            UpdateResult updateResult = await this._mongoCollection.UpdateManyAsync(
                expression, 
                updateDefinition, 
                cancellationToken: cancellationToken);
            SaveResult saveResult = updateResult.ToSaveResult();

            return saveResult;
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await this._mongoCollection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            if (entities.Any())
            {
                await this._mongoCollection.InsertManyAsync(entities, cancellationToken: cancellationToken);
            }
        }

        public async Task ReplaceAsync(Expression<Func<T, bool>> expression, T entity, CancellationToken cancellationToken = default)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ReplaceOneResult result = await this._mongoCollection.ReplaceOneAsync(expression, entity, cancellationToken: cancellationToken);
            
            if (!result.IsAcknowledged)
            {
                throw new ReplaceException("The operation was not acknowledged.");
            }
            else if (result.MatchedCount == 0)
            {
                throw new ReplaceException("No record matched the expression.");
            }
            else if (result.ModifiedCount == 0)
            {
                throw new ReplaceException("No record replaced.");
            }
        }
    }
}