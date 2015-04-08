using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace MongoDb.Migrations.DotNet
{
    public class DbUpgrader
    {
        private const string ScriptsCollectionName = "MigrationScript";
        private MongoDatabase _database;
        private MongoClient _client;
        public DbUpgrader()
        {
            _database = ConnectionUtils.GetMongoDatabase(out _client);
        }

        public static List<MigrationScript> GetMigrationScripts(Assembly scriptAssembly)
        {
            var scriptType = typeof(MigrationScript);
            var scriptTypes = scriptAssembly.GetTypes().Where(t => t.IsClass && t.IsSubclassOf(scriptType)).OrderBy(t => t.Name).ToList();
            return scriptTypes.Select(type => (MigrationScript)Activator.CreateInstance(type)).ToList();
        }

        public void Run<T>() where T : MigrationScript
        {
            var scripts = GetMigrationScripts(typeof(T).Assembly);

            var collection = _database.GetCollection<MigrationScript>(ScriptsCollectionName);            
            foreach (var script in scripts)
            {
                if (!collection.AsQueryable().Any(x=>x.Name==script.Name))
                {
                    script.Execute(_client, _database);
                    _database.GetCollection<MigrationScript>(ScriptsCollectionName).Save(script);                    
                }
            }
        }
    }
}