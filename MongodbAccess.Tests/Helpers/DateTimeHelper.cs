using System;

namespace MongodbAccess.Tests.Helpers
{
    public static class DateTimeHelper
    {
        public static bool AreEquals(DateTime firstDateTime, DateTime secondDateTime)
        {
            return firstDateTime.Year == secondDateTime.Year &&
                firstDateTime.Month == secondDateTime.Month &&
                firstDateTime.Day == secondDateTime.Day &&
                firstDateTime.Minute == secondDateTime.Minute &&
                firstDateTime.Second == secondDateTime.Second;
        }
    }
}