using MongoDB.Driver;
using RepositoryAbstractions;
using System;
using System.Threading.Tasks;

namespace MongodbAccess.Services
{
    public class MongoDbTransaction : ITransaction
    {
        private readonly IClientSessionHandle _clientSessionHandle;

        public MongoDbTransaction(IClientSessionHandle clientSessionHandle)
        {
            this._clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(clientSessionHandle));
        }

        public Task AbortTransactionAsync()
        {
            return this._clientSessionHandle.AbortTransactionAsync();
        }

        public Task CommitTransactionAsync()
        {
            return this._clientSessionHandle.CommitTransactionAsync();
        }

        public Task StartTransactionAsync()
        {
            this._clientSessionHandle.StartTransaction();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this._clientSessionHandle.Dispose();
        }
    }
}
