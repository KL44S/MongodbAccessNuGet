namespace MongodbAccess.Model
{
    public class MongodbConfig
    {
        public string ConnectionString { get; set; }
        public string DBName { get; set; }

        public MongodbConfig(string connectionString, string dbName)
        {
            this.ConnectionString = connectionString;
            this.DBName = dbName;
        }
    }
}
