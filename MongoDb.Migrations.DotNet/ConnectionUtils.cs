using System.Configuration;
using MongoDB.Driver;

namespace MongoDb.Migrations.DotNet
{
    public class ConnectionUtils
    {
        public static MongoDatabase GetMongoDatabase(out MongoClient client)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            client = new MongoClient(connectionString);
            var database = client.GetServer().GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
            return database;
        }  
    }
}