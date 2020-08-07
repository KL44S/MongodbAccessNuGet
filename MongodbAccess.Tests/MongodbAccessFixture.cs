using MongoDB.Driver;
using MongodbAccess.Implementations;
using MongodbAccess.Model;
using MongodbAccess.Services;
using MongodbAccess.Tests.Helpers;
using MongodbAccess.Tests.Models;
using RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MongodbAccess.Tests
{
    public class MongodbAccessFixture
    {
        [Fact]
        public async Task StringSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            IList<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count == 2);
        }

        [Fact]
        public async Task BoolSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = new Sort<Test, bool>(
                                        x => x.BoolField,
                                        SortType.Desc);

            IList<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count == 2);
        }

        [Fact]
        public async Task DatetimeSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, DateTime> sort = new Sort<Test, DateTime>(
                                        x => x.TimestampField,
                                        SortType.Asc);

            IList<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count == 2);
        }

        [Fact]
        public async Task NumberSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, double> sort = new Sort<Test, double>(
                                        x => x.NumberField,
                                        SortType.Asc);

            IList<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count == 2);
        }
    }
}