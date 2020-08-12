using MongoDB.Driver;
using MongodbAccess.Model;
using RepositoryAccess;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess.Services
{
    public interface IMongodbSaveRepository<T> : ISaveRepository<T> where T : IdObject
    {
        Task<bool> UpdateMany(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDefinition);
    }
}
