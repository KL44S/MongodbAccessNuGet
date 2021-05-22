using MongodbAccess.Model;
using System;
using System.Text.RegularExpressions;

namespace MongodbAccess.Tests.Helpers
{
    public static class MongoDbHelper
    {
        public static MongodbConfig GetMongodbConfig()
        {
            MongodbConfig mongodbConfig = new MongodbConfig();

            string mongoConnString = Environment.GetEnvironmentVariable("mongo_db_conn_string_test");
            string regex = @"^.*/(?<dbname>.*)\?";

            Match match = Regex.Match(mongoConnString, regex);

            if (match.Success)
            {
                string dbname = match.Groups["dbname"].ToString();

                mongodbConfig.ConnectionString = mongoConnString;
                mongodbConfig.DBName = dbname;
            }

            return mongodbConfig;
        }
    }
}