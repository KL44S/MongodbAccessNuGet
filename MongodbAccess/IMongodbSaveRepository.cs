using MongoDB.Driver;
using RepositoryAccess;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess
{
    public interface IMongodbSaveRepository<T> : ISaveRepository<T>
    {
        Task<bool> UpdateManyAsync(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition);
    }
}