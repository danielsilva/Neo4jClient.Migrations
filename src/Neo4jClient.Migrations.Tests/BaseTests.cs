using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations.Tests
{
    public class BaseTests : IDisposable
    {
        protected IGraphClient graphClient;

        public BaseTests()
        {
            var client = new GraphClient(new Uri("http://localhost:7474/db/data/"));
            client.Connect();
            graphClient = client;
        }

        public void Dispose()
        {
            ClearDatabase();
        }

        private void ClearDatabase()
        {
            graphClient.Cypher
                .Start(new { n = "node(*)" })
                .OptionalMatch("n-[r]->()")
                .Delete("n, r")
                .ExecuteWithoutResults();
        }
    }
}
