using MongoDB.Driver;
using RepositoryAccess;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess.Services
{
    public interface IMongodbSaveRepository<T> : ISaveRepository<T>
    {
        Task<bool> UpdateMany(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition);
    }
}
