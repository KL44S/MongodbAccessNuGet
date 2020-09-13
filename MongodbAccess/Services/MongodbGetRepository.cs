using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongodbAccess.Services;
using RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess.Implementations
{
    public class MongodbGetRepository<T> : MongodbRepository<T>, IGetRepository<T>
    {
        public MongodbGetRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public async Task<IList<T>> GetAllAsync()
        {
            IList<T> entities = await this._mongoCollection.AsQueryable().ToListAsync();

            return entities;
        }

        public async Task<IList<T>> GetAllByConditionsAsync(Expression<Func<T, bool>> expression)
        {
            IList<T> entities = await this.GetQueryableByConditions(expression).ToListAsync();

            return entities;
        }

        public async Task<IList<T>> GetAllByConditionsAsync<TKey>(Expression<Func<T, bool>> expression, Sort<T, TKey> sort)
        {
            IList<T> entities = await this.GetQueryableByConditionsAndSort(expression, sort).ToListAsync();

            return entities;
        }

        public async Task<IList<T>> GetAllByConditionsAsync<TKey>(Expression<Func<T, bool>> expression, Sort<T, TKey> sort, Pagination pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            if (pagination.TakeNumber < 0)
            {
                throw new ArgumentException(nameof(pagination.TakeNumber));
            }

            if (pagination.SkipNumber < 0)
            {
                throw new ArgumentException(nameof(pagination.SkipNumber));
            }

            IList<T> entities = await this.GetQueryableByConditionsAndSort(expression, sort).Skip(pagination.SkipNumber).Take(pagination.TakeNumber).ToListAsync();

            return entities;
        }

        private IMongoQueryable<T> GetQueryableByConditions(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            IMongoQueryable<T> entities = this._mongoCollection.AsQueryable().Where(expression);

            return entities;
        }

        private IMongoQueryable<T> GetQueryableByConditionsAndSort<TKey>(Expression<Func<T, bool>> expression, Sort<T, TKey> sort)
        {
            if (sort == null)
            {
                throw new ArgumentNullException(nameof(sort));
            }

            IMongoQueryable<T> entities = this.GetQueryableByConditions(expression);

            switch (sort.SortType)
            {
                case SortType.Asc:
                    entities = this._mongoCollection.AsQueryable().Where(expression).OrderBy(sort.SortExpression);
                    break;

                case SortType.Desc:
                    entities = this._mongoCollection.AsQueryable().Where(expression).OrderByDescending(sort.SortExpression);
                    break;
                default:
                    throw new ArgumentException();
            }

            return entities;
        }
    }
}
