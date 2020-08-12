using MongodbAccess.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongodbAccess.Tests.Helpers
{
    public static class TestHelper
    {
        private static readonly Random Random = new Random();

        public static IList<Test> CreateRandomTests(int testsNumber)
        {
            IList<Test> tests = new List<Test>();

            for (int i = 0; i < testsNumber; i++)
            {
                Test test = CreateRandomTest();
                tests.Add(test);
            }

            return tests;
        }

        public static Test CreateRandomTest()
        {
            Test test = new Test()
            {
                BoolField = CreateRandomBool(),
                NumberField = Random.NextDouble(),
                StringField = CreateRandomString(),
                TimestampField = CreateRandomDatetime()
            };

            return test;
        }

        public static string CreateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, Random.Next(50))
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static bool CreateRandomBool()
        {
            int probability = Random.Next(100);

            return probability <= 20;
        }

        public static DateTime CreateRandomDatetime()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(Random.Next(range));
        }
    }
}