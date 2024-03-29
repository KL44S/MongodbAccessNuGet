﻿using MongoDB.Driver;
using MongodbAccess.Services;
using RepositoryAccess;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MongodbAccess.Implementations
{
    public class MongodbDeleteRepository<T> : MongodbRepository<T>, IDeleteRepository<T>
    {
        public MongodbDeleteRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public async Task DeleteAllByConditionsAsync(
            Expression<Func<T, bool>> expression, 
            CancellationToken cancellationToken = default)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            await this._mongoCollection.DeleteManyAsync(expression, cancellationToken);
        }
    }
}