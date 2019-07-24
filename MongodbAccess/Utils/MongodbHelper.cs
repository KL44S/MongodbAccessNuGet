namespace MongodbAccess.Helpers
{
    internal class MongodbHelper
    {
        internal static string GetCollectionName<T>()
        {
            string collectionName = typeof(T).Name;
            collectionName = $"{char.ToLowerInvariant(collectionName[0])}{collectionName.Substring(1)}s";

            return collectionName;
        }

    }
}
