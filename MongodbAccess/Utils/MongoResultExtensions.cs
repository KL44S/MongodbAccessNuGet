using MongoDB.Driver;
using MongodbAccess.Model;

namespace MongodbAccess.Utils
{
    public static class MongoResultExtensions
    {
        public static SaveResult ToSaveResult(this UpdateResult updateResult)
        {
            bool isAcknowledge = updateResult.IsAcknowledged;
            long? matchedCount = updateResult.IsAcknowledged ? (long?)updateResult.MatchedCount : null;
            long? modifiedCount = updateResult.IsAcknowledged ? (long?)updateResult.ModifiedCount : null;

            return new SaveResult(isAcknowledge, matchedCount, modifiedCount);
        }
    }
}