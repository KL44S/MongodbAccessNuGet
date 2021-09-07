using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RepositoryAbstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MongodbAccess.Services
{
    internal class MongodbQueryable<T> : IRepositoryQueryable<T>
    {
        private readonly IMongoQueryable<T> _mongoQueryable;

        public MongodbQueryable(IMongoQueryable<T> mongoQueryable)
        {
            this._mongoQueryable = mongoQueryable ?? throw new ArgumentNullException(nameof(mongoQueryable));
        }

        public Type ElementType => this._mongoQueryable.ElementType;

        public Expression Expression => this._mongoQueryable.Expression;

        public IQueryProvider Provider => this._mongoQueryable.Provider;

        public IEnumerator<T> GetEnumerator()
        {
            return this._mongoQueryable.GetEnumerator();
        }
        
        public Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            return this._mongoQueryable.ToListAsync(cancellationToken);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._mongoQueryable.GetEnumerator();
        }
    }
}
