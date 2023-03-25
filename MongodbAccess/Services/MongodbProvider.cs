using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongodbAccess.Model;

namespace MongodbAccess.Services
{
    public class MongodbProvider
    {
        private static IDictionary<string, MongoClient> MongoClients = new Dictionary<string, MongoClient>();
        private static IDictionary<string, IMongoDatabase> Databases = new Dictionary<string, IMongoDatabase>();

        public static IMongoDatabase GetDatabase(MongodbConfig mongodbConfig)
        {
            ValidateParams(mongodbConfig);

            string databaseKey = mongodbConfig.ConnectionString + mongodbConfig.DBName;

            MongoClient mongoClient = null;
            IMongoDatabase database = null;

            if (MongoClients.ContainsKey(mongodbConfig.ConnectionString))
            {
                if (Databases.ContainsKey(databaseKey))
                {
                    database = Databases[databaseKey];
                }
                else
                {
                    mongoClient = MongoClients[mongodbConfig.ConnectionString];

                    database = mongoClient.GetDatabase(mongodbConfig.DBName);
                    Databases.Add(databaseKey, database);
                }
            }
            else
            {
                mongoClient = new MongoClient(mongodbConfig.ConnectionString);
                MongoClients.Add(mongodbConfig.ConnectionString, mongoClient);

                database = mongoClient.GetDatabase(mongodbConfig.DBName);
                Databases.Add(databaseKey, database);
            }

            return database;
        }

        public static MongoClient? GetMongoClient(MongodbConfig mongodbConfig)
        {
            ArgumentNullException.ThrowIfNull(mongodbConfig, nameof(mongodbConfig));

            MongoClient? mongoClient = null;

            if (MongoClients.ContainsKey(mongodbConfig.ConnectionString))
            {
                mongoClient = MongoClients[mongodbConfig.ConnectionString];
            }

            return mongoClient;
        }

        private static void ValidateParams(MongodbConfig mongodbConfig)
        {
            if (mongodbConfig == null)
            {
                throw new ArgumentNullException("The config provided can't be null");
            }

            if (string.IsNullOrEmpty(mongodbConfig.ConnectionString))
            {
                throw new ArgumentNullException("The config provided can't be null");
            }

            if (string.IsNullOrEmpty(mongodbConfig.DBName))
            {
                throw new ArgumentNullException("The config provided can't be null");
            }
        }
    }
}
