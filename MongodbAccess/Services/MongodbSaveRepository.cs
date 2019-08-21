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

        public async Task SaveAsync(T entity)
        {
            await this._mongoCollection.InsertOneAsync(entity);
        }

        public async Task SaveAsync(IList<T> entities)
        {
            await this._mongoCollection.InsertManyAsync(entities);
        }

        public async Task UpdateAsync(Expression<Func<T, bool>> expression, T entity)
        {
            await this._mongoCollection.ReplaceOneAsync(expression, entity);
        }

        public async Task UpdateMany(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition)
        {
            await this._mongoCollection.UpdateManyAsync(expression, updateDefinition);
        }
    }
}
