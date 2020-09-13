namespace MongodbAccess.Model
{
    public class SaveResult
    {
        public SaveResult(
            bool isAcknowledged,
            long? matchedCount = null,
            long? modifiedCount = null)
        {
            IsAcknowledged = isAcknowledged;
            MatchedCount = matchedCount;
            ModifiedCount = modifiedCount;
        }

        //
        // Summary:
        //     Gets a value indicating whether the result is acknowledged.
        public bool IsAcknowledged { get; }

        //
        // Summary:
        //     Gets the matched count. If IsAcknowledged is false, this will throw an exception.
        public long? MatchedCount { get; }

        //
        // Summary:
        //     Gets the modified count. If IsAcknowledged is false, this will throw an exception.
        public long? ModifiedCount { get; }
    }
}