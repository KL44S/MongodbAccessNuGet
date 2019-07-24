using MongoDB.Driver;
using MongodbAccess.Services;
using RepositoryAccess;
using System;
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

        public async Task UpdateAsync(Expression<Func<T, bool>> expression, T entity)
        {
            await this.MongoCollection.ReplaceOneAsync(expression, entity);
        }
    }
}
