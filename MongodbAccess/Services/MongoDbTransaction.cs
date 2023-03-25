using MongoDB.Driver;
using RepositoryAbstractions;
using System;
using System.Threading;
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

        public Task AbortTransactionAsync(CancellationToken cancellationToken)
        {
            return this._clientSessionHandle.AbortTransactionAsync(cancellationToken);
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            return this._clientSessionHandle.CommitTransactionAsync(cancellationToken);
        }

        public Task StartTransactionAsync(CancellationToken cancellationToken)
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
