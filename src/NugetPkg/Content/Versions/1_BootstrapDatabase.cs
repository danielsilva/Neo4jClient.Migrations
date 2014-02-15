using Neo4jClient;
using Neo4jClient.Migrations;

namespace Migrations.Versions
{
    [Migration(1)]
    public class _1_BootstrapDatabase : IMigration
    {
        public void Up(IGraphClient graphClient)
        {
            // TODO: put your specific migration code here
            graphClient.Cypher
                .Create("(m:MovieType {movieType})")
                .WithParams(new { movieType = new { Name = "Science Fiction" } })
                .ExecuteWithoutResults();
        }

        public void Down(IGraphClient graphClient)
        {
            // TODO: put your specific migration code here
            graphClient.Cypher
                .Match("(m:MovieType)")
                .Where("m.Name = 'Science Fiction'")
                .Delete("m")
                .ExecuteWithoutResults();
        }
    }
}
