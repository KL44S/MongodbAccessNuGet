using MongoDB.Driver;
using MongodbAccess.Model;
using System.Collections.Generic;

namespace MongodbAccess.Services
{
    public class MongodbProvider
    {
        private static IDictionary<string, MongoClient> MongoClients = new Dictionary<string, MongoClient>();
        private static IDictionary<string, IMongoDatabase> Databases = new Dictionary<string, IMongoDatabase>();

        public static IMongoDatabase GetDatabase(MongodbConfig mongodbConfig)
        {
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
    }
}
