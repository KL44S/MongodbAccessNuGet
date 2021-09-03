using MongodbAccess.Tests.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MongodbAccess.Tests.Services
{
    public class TestsComparer : IEqualityComparer<Test>
    {
        public bool Equals(Test x, Test y)
        {
            return x.ObjectId == y.ObjectId;
        }

        public int GetHashCode([DisallowNull] Test obj)
        {
            return int.Parse(obj.ObjectId);
        }
    }
}
