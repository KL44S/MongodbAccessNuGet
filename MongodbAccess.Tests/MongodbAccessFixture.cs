using MongoDB.Driver;
using MongodbAccess.Implementations;
using MongodbAccess.Model;
using MongodbAccess.Services;
using MongodbAccess.Tests.Helpers;
using MongodbAccess.Tests.Models;
using RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task ModifyingExistingTestTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);
            ISaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);

            Sort<Test, double> sort = new Sort<Test, double>(
                                        x => x.NumberField,
                                        SortType.Asc);

            IList<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 1));
            Test test = tests.Single();
            var objectId = test.ObjectId;

            var testString = TestHelper.CreateRandomString();
            test.StringField = testString;

            var result = await saveRepository.UpdateAsync(test);

            Assert.True(result);

            tests = await getRepository.GetAllByConditionsAsync(x => x.StringField == testString);

            try
            {
                test = tests.Single();
                Assert.Equal(test.ObjectId, objectId);
            }
            catch
            {
                Assert.True(false, "Not single Test object obtained");
            }
        }

        [Fact]
        public async Task ModifyingExistingTestsTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);
            ISaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);

            Sort<Test, double> sort = new Sort<Test, double>(
                                        x => x.NumberField,
                                        SortType.Asc);

            IList<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 3));

            var testString = TestHelper.CreateRandomString();
            var testIds = tests.Select(x => x.ObjectId);

            foreach(Test test in tests)
            {
                test.StringField = testString;
            }

            var result = await saveRepository.UpdateAsync(tests);

            Assert.True(result.SuccessfulIds.All(x => testIds.Contains(x)) 
                && result.FailedIds.Count == 0);
        }
    }
}