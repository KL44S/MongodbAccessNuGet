using MongoDB.Driver;
using MongodbAccess.Implementations;
using MongodbAccess.Model;
using MongodbAccess.Services;
using MongodbAccess.Tests.Helpers;
using MongodbAccess.Tests.Models;
using RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MongodbAccess.Tests
{
    public class PerformanceTests
    {
        [Fact(Skip="Only for manual testing purposes.")]
        public async Task InsertManyVsInsertOne()
        {
            MongodbConfig mongodbConfig = MongoDbHelper.GetMongodbConfig();
            IMongoDatabase database = MongodbProvider.GetDatabase(mongodbConfig);
            ISaveRepository<Test> saveRepository = new MongodbSaveRepository<Test>(database);
            Stopwatch stopwatch = new Stopwatch();

            IList<Test> tests = TestHelper.CreateRandomTests(100);

            // First insert in order establish the connection
            await saveRepository.InsertAsync(tests.ElementAt(0));

            // One-by-one insert
            stopwatch.Restart();
            foreach (Test test in tests)
            {
                await saveRepository.InsertAsync(test);
            }
            stopwatch.Stop();
            TimeSpan oneByOneTime = stopwatch.Elapsed;

            // Multi-thread insert
            stopwatch.Restart();
            IList<Task> tasks = new List<Task>();
            foreach (Test test in tests)
            {
                tasks.Add(saveRepository.InsertAsync(test));
            }
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            TimeSpan multiThreadTime = stopwatch.Elapsed;

            // Insert many (bulk)
            stopwatch.Restart();
            await saveRepository.InsertManyAsync(tests);
            stopwatch.Stop();
            TimeSpan bulkTime = stopwatch.Elapsed;

            Assert.True(
                TimeSpan.Compare(oneByOneTime, multiThreadTime) == 1 &&
                TimeSpan.Compare(multiThreadTime, bulkTime) == 1);
        }
    }
}