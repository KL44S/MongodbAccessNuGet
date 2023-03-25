using MongoDB.Driver;
using MongodbAccess.Model;
using RepositoryAbstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongodbAccess.Services
{
    public class MongoDbTransactionFactory : ITransactionFactory
    {
        private MongodbConfig _mongoDbConfig;

        public MongoDbTransactionFactory(MongodbConfig mongoDbConfig)
        {
            this._mongoDbConfig = mongoDbConfig ?? throw new ArgumentNullException(nameof(mongoDbConfig));
        }

        public async Task<ITransaction> CreateTransactionAsync(CancellationToken cancellationToken)
        {
            MongoClient? mongoClient = MongodbProvider.GetMongoClient(this._mongoDbConfig);

            if (mongoClient != null)
            {
                IClientSessionHandle clientSessionHandle = await mongoClient.StartSessionAsync(cancellationToken: cancellationToken);

                return new MongoDbTransaction(clientSessionHandle);
            }

            throw new NullReferenceException("Couldn't find any Mongo DB Client.");
        }
    }
}
