using MongoDB.Driver;
using MongodbAccess.Implementations;
using MongodbAccess.Model;
using MongodbAccess.Services;
using MongodbAccess.Tests.Helpers;
using MongodbAccess.Tests.Models;
using RepositoryAbstractions.Exceptions;
using RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MongodbAccess.Tests
{
    public class MongodbAccessFixture
    {
        #region Get tests

        [Fact]
        public async Task NoResultsOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            IEnumerable<Test> tests = await getRepository.GetAllByConditionsAsync(x => x.ObjectId == "unexistent-object");

            Assert.Empty(tests);
        }

        [Fact]
        public async Task NullExpressionOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await getRepository.GetAllByConditionsAsync(null));
        }

        [Fact]
        public async Task NullExpressionWithSortOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        null,
                                                        sort));
        }

        [Fact]
        public async Task NullExpressionWithSortAndPaginationOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        null,
                                                        sort,
                                                        new Pagination(0, 2)));
        }

        [Fact]
        public async Task NullSortOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort));
        }

        [Fact]
        public async Task NullSortWithPaginationOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)));
        }

        [Fact]
        public async Task NullPaginationOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                          x => x.StringField,
                                          SortType.Desc);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        null));
        }

        [Fact]
        public async Task InvalidPaginationSkipNumberTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                          x => x.StringField,
                                          SortType.Desc);

            await Assert.ThrowsAsync<ArgumentException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(-5, 2)));
        }

        [Fact]
        public async Task InvalidPaginationTakeNumberTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                          x => x.StringField,
                                          SortType.Desc);

            await Assert.ThrowsAsync<ArgumentException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, -2)));
        }

        [Fact]
        public async Task StringDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            IEnumerable<Test> sortedTests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(sortedTests != null && sortedTests.Count() == 2);
        }

        [Fact]
        public async Task StringDescSortAndPaginationCancelledTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(0.1));

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2),
                                                        cancellationTokenSource.Token));
        }

        [Fact]
        public async Task StringAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Asc);

            IEnumerable<Test> sortedTests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(sortedTests != null && sortedTests.Count() == 2);
        }

        [Fact]
        public async Task BoolDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = new Sort<Test, bool>(
                                        x => x.BoolField,
                                        SortType.Desc);

            IEnumerable<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count() == 2);

            IEnumerable<Test> sortedTests = tests.OrderByDescending(x => x.BoolField);

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public async Task BoolAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = new Sort<Test, bool>(
                                        x => x.BoolField,
                                        SortType.Asc);

            IEnumerable<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count() == 2);

            IEnumerable<Test> sortedTests = tests.OrderBy(x => x.BoolField);

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public async Task DatetimeDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, DateTime> sort = new Sort<Test, DateTime>(
                                        x => x.TimestampField,
                                        SortType.Desc);

            IEnumerable<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count() == 2);

            IEnumerable<Test> sortedTests = tests.OrderByDescending(x => x.TimestampField);

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public async Task DatetimeAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, DateTime> sort = new Sort<Test, DateTime>(
                                        x => x.TimestampField,
                                        SortType.Asc);

            IEnumerable<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count() == 2);

            IEnumerable<Test> sortedTests = tests.OrderBy(x => x.TimestampField);

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public async Task NumberDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, double> sort = new Sort<Test, double>(
                                        x => x.NumberField,
                                        SortType.Desc);

            IEnumerable<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count() == 2);

            IEnumerable<Test> sortedTests = tests.OrderByDescending(x => x.NumberField);

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public async Task NumberAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, double> sort = new Sort<Test, double>(
                                        x => x.NumberField,
                                        SortType.Asc);

            IEnumerable<Test> tests = await getRepository.GetAllByConditionsAsync(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2));

            Assert.True(tests != null && tests.Count() == 2);

            IEnumerable<Test> sortedTests = tests.OrderBy(x => x.NumberField);

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        #endregion

        #region Save tests

        [Fact]
        public async Task NullEntityOnInsertTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IMongodbSaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);

            await Assert.ThrowsAsync<ArgumentNullException>(() => saveRepository.InsertAsync(null));
        }

        [Fact]
        public async Task NullEntityOnReplaceTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IMongodbSaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);

            await Assert.ThrowsAsync<ArgumentNullException>(() => saveRepository.ReplaceAsync(x => true, null));
        }

        [Fact]
        public async Task NullExpressionOnReplaceTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IMongodbSaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);

            await Assert.ThrowsAsync<ArgumentNullException>(() => saveRepository.ReplaceAsync(null, new Test()));
        }

        [Fact]
        public async Task NullEntitiesOnInsertManyTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IMongodbSaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);

            await Assert.ThrowsAsync<ArgumentNullException>(() => saveRepository.InsertManyAsync(null));
        }

        [Fact]
        public async Task NothingToReplaceTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IMongodbSaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);

            await Assert.ThrowsAsync<ReplaceException>(() => saveRepository.ReplaceAsync(x => x.ObjectId == "non-existent-object", new Test() { StringField = "nothing-to-replace-test" }));
        }

        #endregion

        #region Delete tests

        [Fact]
        public async Task NullExpressionOnDeleteTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IDeleteRepository<Test> deleteRepository = new MongodbDeleteRepository<Test>(database);

            await Assert.ThrowsAsync<ArgumentNullException>(() => deleteRepository.DeleteAllByConditionsAsync(null));
        }

        [Fact]
        public async Task NothingToDeleteTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IDeleteRepository<Test> deleteRepository = new MongodbDeleteRepository<Test>(database);

            await deleteRepository.DeleteAllByConditionsAsync(x => x.ObjectId == "non-existent-object");
        }

        #endregion

        #region Integral tests

        [Fact]
        public async Task SingleEntityIntegralTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IMongodbSaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);
            IDeleteRepository<Test> deleteRepository = new MongodbDeleteRepository<Test>(database);

            Test newTest = new Test()
            {
                BoolField = true,
                NumberField = 1,
                ObjectId = Guid.NewGuid().ToString(),
                StringField = "new-single-object",
                TimestampField = DateTime.UtcNow
            };

            await saveRepository.InsertAsync(newTest);

            Test savedTest = await getRepository.GetFirstByConditionsAsync(x =>
                                x.ObjectId == newTest.ObjectId &&
                                x.BoolField == newTest.BoolField &&
                                x.NumberField == newTest.NumberField &&
                                x.StringField == newTest.StringField &&
                                x.TimestampField == newTest.TimestampField);

            savedTest.BoolField = false;
            savedTest.NumberField = 2;
            savedTest.StringField = "modified-single-object";
            savedTest.TimestampField = DateTime.UtcNow.AddDays(1);

            await saveRepository.ReplaceAsync(
                                x =>
                                    x.ObjectId == newTest.ObjectId &&
                                    x.BoolField == newTest.BoolField &&
                                    x.NumberField == newTest.NumberField &&
                                    x.StringField == newTest.StringField &&
                                    x.TimestampField == newTest.TimestampField,
                                savedTest);

            Test modifiedTest = (await getRepository.GetAllByConditionsAsync(x =>
                                    x.ObjectId == savedTest.ObjectId &&
                                    x.BoolField == savedTest.BoolField &&
                                    x.NumberField == savedTest.NumberField &&
                                    x.StringField == savedTest.StringField &&
                                    x.TimestampField == savedTest.TimestampField)).Single();

            await deleteRepository.DeleteAllByConditionsAsync(x =>
                                x.ObjectId == savedTest.ObjectId &&
                                x.BoolField == savedTest.BoolField &&
                                x.NumberField == savedTest.NumberField &&
                                x.StringField == savedTest.StringField &&
                                x.TimestampField == savedTest.TimestampField);

            Test deletedTest = await getRepository.GetFirstByConditionsAsync(x =>
                                   x.ObjectId == savedTest.ObjectId &&
                                   x.BoolField == savedTest.BoolField &&
                                   x.NumberField == savedTest.NumberField &&
                                   x.StringField == savedTest.StringField &&
                                   x.TimestampField == savedTest.TimestampField);

            Assert.Null(deletedTest);
        }

        [Fact]
        public async Task MultipleEntityIntegralTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IMongodbSaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);
            IDeleteRepository<Test> deleteRepository = new MongodbDeleteRepository<Test>(database);

            Test newTest1 = new Test()
            {
                BoolField = true,
                NumberField = 1,
                ObjectId = Guid.NewGuid().ToString(),
                StringField = "multiple-object-1",
                TimestampField = DateTime.UtcNow
            };

            Test newTest2 = new Test()
            {
                BoolField = true,
                NumberField = 2,
                ObjectId = Guid.NewGuid().ToString(),
                StringField = "multiple-object-2",
                TimestampField = DateTime.UtcNow.AddMinutes(1)
            };

            IList<Test> newTests = new List<Test>()
            {
                newTest1, newTest2
            };

            await saveRepository.InsertManyAsync(newTests);

            IEnumerable<Test> savedTests = await getRepository.GetAllByConditionsAsync(x =>
                                x.ObjectId == newTest1.ObjectId ||
                                x.ObjectId == newTest2.ObjectId);

            Assert.True(savedTests.Count() == 2);

            Test savedTest1 = savedTests.Single(x =>
                x.ObjectId == newTest1.ObjectId &&
                x.NumberField == newTest1.NumberField &&
                x.StringField == newTest1.StringField &&
                DateTimeHelper.AreEquals(x.TimestampField, newTest1.TimestampField) &&
                x.BoolField == newTest1.BoolField);

            Test savedTest2 = savedTests.Single(x =>
                x.ObjectId == newTest2.ObjectId &&
                x.NumberField == newTest2.NumberField &&
                x.StringField == newTest2.StringField &&
                DateTimeHelper.AreEquals(x.TimestampField, newTest2.TimestampField) &&
                x.BoolField == newTest2.BoolField);

            string modifiedStringField = "modified-multiple-objects";
            UpdateDefinition<Test> updateDefinition = Builders<Test>.Update.Set(x => x.StringField, modifiedStringField);

            await saveRepository.UpdateManyAsync(
                                x =>
                                    x.ObjectId == newTest1.ObjectId ||
                                    x.ObjectId == newTest2.ObjectId,
                                updateDefinition);

            IEnumerable<Test> modifiedTests = await getRepository.GetAllByConditionsAsync(x => 
                                            x.StringField == modifiedStringField && 
                                            (x.ObjectId == savedTest1.ObjectId || x.ObjectId == savedTest2.ObjectId));

            Assert.True(modifiedTests.Count() == 2);

            Test modifiedTest1 = modifiedTests.Single(x =>
                x.ObjectId == newTest1.ObjectId &&
                x.NumberField == newTest1.NumberField &&
                x.StringField == modifiedStringField &&
                 DateTimeHelper.AreEquals(x.TimestampField, newTest1.TimestampField) &&
                x.BoolField == newTest1.BoolField);

            Test modifiedTest2 = modifiedTests.Single(x =>
                x.ObjectId == newTest2.ObjectId &&
                x.NumberField == newTest2.NumberField &&
                x.StringField == modifiedStringField &&
                 DateTimeHelper.AreEquals(x.TimestampField, newTest2.TimestampField) &&
                x.BoolField == newTest2.BoolField);

            await deleteRepository.DeleteAllByConditionsAsync(x =>
                                x.StringField == modifiedStringField);

            IEnumerable<Test> deletedTests = await getRepository.GetAllByConditionsAsync(x => x.StringField == modifiedStringField);

            Assert.True(deletedTests == null || deletedTests.Count() == 0);
        }

        #endregion
    }
}