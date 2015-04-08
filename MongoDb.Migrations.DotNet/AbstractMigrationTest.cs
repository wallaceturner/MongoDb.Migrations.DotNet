using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using MongoDB.Driver;

namespace MongoDb.Migrations.DotNet
{
    public abstract class AbstractMigrationTest
    {
        protected MongoDatabase _database;
        protected MongoClient _client;

        protected AbstractMigrationTest()
        {
            _database = ConnectionUtils.GetMongoDatabase(out _client);
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;            
            _client.GetServer().DropDatabase(databaseName);
        }

        //protected void Test<T>(Action<MongoClient> OnBeforeExecute, Action<MongoDatabase> OnAfterExecute) where T : MigrationScript
        //{
            
        //}

        
    }
}