using MongoDB.Driver;
using MongodbAccess.Model;
using RepositoryAccess;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess
{
    public interface IMongodbSaveRepository<T> : ISaveRepository<T>
    {
        Task<SaveResult> UpdateManyAsync(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition);
    }
}