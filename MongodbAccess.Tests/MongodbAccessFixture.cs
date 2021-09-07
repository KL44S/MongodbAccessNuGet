using MongoDB.Driver;
using MongodbAccess.Implementations;
using MongodbAccess.Model;
using MongodbAccess.Services;
using MongodbAccess.Tests.Helpers;
using MongodbAccess.Tests.Models;
using MongodbAccess.Tests.Services;
using RepositoryAbstractions.Exceptions;
using RepositoryAbstractions.Extensions;
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
        private readonly IEqualityComparer<Test> _equalityComparer = new TestsComparer();

        #region Get tests

        [Fact]
        public void NoResultsOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            IList<Test> tests = getRepository.GetAllByConditions(x => x.ObjectId == "unexistent-object").ToList();

            Assert.Empty(tests);
        }

        [Fact]
        public void NullExpressionOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            Assert.Throws<ArgumentNullException>(() => getRepository.GetAllByConditions(null).ToList());
        }

        [Fact]
        public void NullExpressionWithSortOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            Assert.Throws<ArgumentNullException>(() => getRepository.GetAllByConditions(
                                                        null,
                                                        sort));
        }

        [Fact]
        public void NullExpressionWithSortAndPaginationOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            Assert.Throws<ArgumentNullException>(() => getRepository.GetAllByConditions(
                                                        null,
                                                        sort,
                                                        new Pagination(0, 2)));
        }

        [Fact]
        public void NullSortOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = null;

            Assert.Throws<ArgumentNullException>(() => getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort));
        }

        [Fact]
        public void NullSortWithPaginationOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = null;

            Assert.Throws<ArgumentNullException>(() => getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)));
        }

        [Fact]
        public void NullPaginationOnGetTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                          x => x.StringField,
                                          SortType.Desc);

            Assert.Throws<ArgumentNullException>(() => getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        null));
        }

        [Fact]
        public void InvalidPaginationSkipNumberTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                          x => x.StringField,
                                          SortType.Desc);

            Assert.Throws<ArgumentException>(() => getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(-5, 2)));
        }

        [Fact]
        public void InvalidPaginationTakeNumberTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                          x => x.StringField,
                                          SortType.Desc);

            Assert.Throws<ArgumentException>(() => getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, -2)));
        }

        [Fact]
        public void StringDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            IList<Test> sortedTests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(sortedTests != null && sortedTests.Count == 2);
        }

        [Fact]
        public async Task AsyncStringDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            IList<Test> sortedTests = await getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToListAsync();

            Assert.True(sortedTests != null && sortedTests.Count == 2);
        }

        [Fact]
        public async Task AsyncStringDescSortAndPaginationCancelledTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Desc);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(0.5));

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToListAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public void StringAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, string> sort = new Sort<Test, string>(
                                        x => x.StringField,
                                        SortType.Asc);

            IList<Test> sortedTests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(sortedTests != null && sortedTests.Count == 2);

            IList<Test> memoryTests = getRepository.GetAll().ToList().OrderBy(x => x.StringField).Take(2).ToList();

            Assert.True(sortedTests != null && sortedTests.Count == 2);
            Assert.True(memoryTests != null && memoryTests.Count == 2);

            Assert.True(sortedTests.SequenceEqual(memoryTests, _equalityComparer));
        }

        [Fact]
        public void BoolDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = new Sort<Test, bool>(
                                        x => x.BoolField,
                                        SortType.Desc);

            IList<Test> tests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(tests != null && tests.Count == 2);

            IList<Test> sortedTests = tests.OrderByDescending(x => x.BoolField).ToList();

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public void BoolAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, bool> sort = new Sort<Test, bool>(
                                        x => x.BoolField,
                                        SortType.Asc);

            IList<Test> tests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(tests != null && tests.Count == 2);

            IList<Test> sortedTests = tests.OrderBy(x => x.BoolField).ToList();

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public void DatetimeDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, DateTime> sort = new Sort<Test, DateTime>(
                                        x => x.TimestampField,
                                        SortType.Desc);

            IList<Test> tests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(tests != null && tests.Count == 2);

            IList<Test> sortedTests = tests.OrderByDescending(x => x.TimestampField).ToList();

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public void DatetimeAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, DateTime> sort = new Sort<Test, DateTime>(
                                        x => x.TimestampField,
                                        SortType.Asc);

            IList<Test> tests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(tests != null && tests.Count == 2);

            IList<Test> sortedTests = tests.OrderBy(x => x.TimestampField).ToList();

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public void NumberDescSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, double> sort = new Sort<Test, double>(
                                        x => x.NumberField,
                                        SortType.Desc);

            IList<Test> tests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(tests != null && tests.Count == 2);

            IList<Test> sortedTests = tests.OrderByDescending(x => x.NumberField).ToList();

            Assert.True(tests.SequenceEqual(sortedTests));
        }

        [Fact]
        public void NumberAscSortAndPaginationTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IGetRepository<Test> getRepository = new MongodbGetRepository<Test>(database);

            Sort<Test, double> sort = new Sort<Test, double>(
                                        x => x.NumberField,
                                        SortType.Asc);

            IList<Test> tests = getRepository.GetAllByConditions(
                                                        x => true,
                                                        sort,
                                                        new Pagination(0, 2)).ToList();

            Assert.True(tests != null && tests.Count == 2);

            IList<Test> sortedTests = tests.OrderBy(x => x.NumberField).ToList();

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
        public void NothingToDeleteTest()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            IDeleteRepository<Test> deleteRepository = new MongodbDeleteRepository<Test>(database);

            deleteRepository.DeleteAllByConditionsAsync(x => x.ObjectId == "non-existent-object");
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

            Test modifiedTest = (getRepository.GetAllByConditions(x =>
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

            IList<Test> savedTests = getRepository.GetAllByConditions(x =>
                                x.ObjectId == newTest1.ObjectId ||
                                x.ObjectId == newTest2.ObjectId).ToList();

            Assert.True(savedTests.Count == 2);

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

            IList<Test> modifiedTests = getRepository.GetAllByConditions(x => 
                                            x.StringField == modifiedStringField && 
                                            (x.ObjectId == savedTest1.ObjectId || x.ObjectId == savedTest2.ObjectId)).ToList();

            Assert.True(modifiedTests.Count == 2);

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

            IList<Test> deletedTests = getRepository.GetAllByConditions(x => x.StringField == modifiedStringField).ToList();

            Assert.True(deletedTests == null || deletedTests.Count == 0);
        }

        #endregion
    }
}