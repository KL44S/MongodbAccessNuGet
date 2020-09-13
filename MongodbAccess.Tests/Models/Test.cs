using System;

namespace MongodbAccess.Tests.Models
{
    public class Test
    {
        public string StringField { get; set; }
        public double NumberField { get; set; }
        public DateTime TimestampField { get; set; }
        public bool BoolField { get; set; }
        public string ObjectId { get; set ; }
    }
}