using System;
using System.Reflection;
using Neo4jClient;
using Neo4jClient.Migrations;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate your graph client
            var myGraphDatabase = new GraphClient(new Uri("http://path/to/neo4j/data"));

            // run this code migrate to the latest version
            new Migrator(myGraphDatabase).Migrate(Assembly.GetExecutingAssembly());

            // ...to migrate to a specific version (up or down) pass 'toVersion' parameter
            // new Migrator(myGraphDatabase).Migrate(Assembly.GetExecutingAssembly(), toVersion: 1);
        }
    }
}
