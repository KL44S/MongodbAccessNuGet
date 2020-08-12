using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongodbAccess.Helpers;
using MongodbAccess.Model;

namespace MongodbAccess.Services
{
    public abstract class MongodbRepository<T> where T : IdObject
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
                    cm.MapIdProperty(c => c.ObjectId)
                        .SetIdGenerator(StringObjectIdGenerator.Instance)
                        .SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }
        }
    }
}