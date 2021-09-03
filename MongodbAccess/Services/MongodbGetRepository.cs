using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongodbAccess.Services;
using RepositoryAccess;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongodbAccess.Implementations
{
    public class MongodbGetRepository<T> : MongodbRepository<T>, IGetRepository<T>
    {
        public MongodbGetRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public IQueryable<T> GetAll()
        {
            return this._mongoCollection.AsQueryable();
        }

        public IQueryable<T> GetAllByConditions(Expression<Func<T, bool>> expression)
        {
            return this.GetQueryableByConditions(expression);
        }

        public IQueryable<T> GetAllByConditions<TKey>(Expression<Func<T, bool>> expression, Sort<T, TKey> sort)
        {
            return this.GetQueryableByConditionsAndSort(expression, sort);
        }

        public IQueryable<T> GetAllByConditions<TKey>(Expression<Func<T, bool>> expression, Sort<T, TKey> sort, Pagination pagination)
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

            IMongoQueryable<T> entities = this.GetQueryableByConditionsAndSort(expression, sort).Skip(pagination.SkipNumber).Take(pagination.TakeNumber);

            return entities;
        }

        public async Task<T> GetFirstByConditionsAsync(Expression<Func<T, bool>> expression)
        {
            T entity = await this._mongoCollection.AsQueryable().FirstOrDefaultAsync(expression);

            return entity;
        }

        public async Task<T> GetFirstByConditionsAsync<TKey>(Expression<Func<T, bool>> expression, Sort<T, TKey> sort)
        {
            IMongoQueryable<T> entities = this.GetQueryableByConditionsAndSort(expression, sort);
            T entity = await entities.FirstOrDefaultAsync();

            return entity;
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
                    entities = entities.OrderBy(sort.SortExpression);
                    break;

                case SortType.Desc:
                    entities = entities.OrderByDescending(sort.SortExpression);
                    break;
                default:
                    throw new ArgumentException();
            }

            return entities;
        }
    }
}
