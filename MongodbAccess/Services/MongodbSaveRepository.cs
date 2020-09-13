using MongoDB.Driver;
using MongodbAccess.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess.Implementations
{
    public class MongodbSaveRepository<T> : MongodbRepository<T>, IMongodbSaveRepository<T>
    {

        public MongodbSaveRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public async Task<bool> UpdateManyAsync(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition)
        {
            var result = await this._mongoCollection.UpdateManyAsync(expression, updateDefinition);

            return (result.IsAcknowledged && result.MatchedCount > 0 && result.ModifiedCount > 0);
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await this._mongoCollection.InsertOneAsync(entity);
        }

        public async Task InsertManyAsync(IList<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            if (entities.Count > 0)
            {
                await this._mongoCollection.InsertManyAsync(entities);
            }
        }

        public async Task ReplaceAsync(Expression<Func<T, bool>> expression, T entity)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await this._mongoCollection.ReplaceOneAsync(expression, entity);
        }
    }
}