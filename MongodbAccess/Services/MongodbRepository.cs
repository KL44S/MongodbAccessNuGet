using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongodbAccess.Helpers;

namespace MongodbAccess.Services
{
    public abstract class MongodbRepository<T>
    {
        protected IMongoCollection<T> _mongoCollection;

        public MongodbRepository(IMongoDatabase mongoDatabase)
        {
            this.RegisterAutoMap();

            string collectionName = MongodbHelper.GetCollectionName<T>();

            this._mongoCollection = mongoDatabase.GetCollection<T>(collectionName);
        }

        private void RegisterAutoMap()
        {
            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}