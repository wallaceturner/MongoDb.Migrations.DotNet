using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDb.Migrations.DotNet
{
    public abstract class MigrationScript
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        protected MigrationScript()
        {
            Name = GetType().Name;
        }

        public string Name { get; set; }

        public void Execute(MongoClient client, MongoDatabase database)
        {
            if (OnBeforeExecute != null)
                OnBeforeExecute(client, database);
            OnExecute(client,database);
            if (OnAfterExecute != null)
                OnAfterExecute(client, database);
        }

        protected abstract void OnExecute(MongoClient client, MongoDatabase database);


        public event Action<MongoClient, MongoDatabase> OnBeforeExecute;
        public event Action<MongoClient, MongoDatabase> OnAfterExecute;
    }
}