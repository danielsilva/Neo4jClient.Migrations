using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations.Tests.Stub
{
    [Migration(1)]
    public class Migration1 : IMigration
    {
        public static Movie Movie 
        { 
            get 
            {
                return new Movie { Title = "The Matrix" };
            } 
        }

        public void Up(IGraphClient graphClient)
        {
            graphClient.Cypher.Create("(m:Movie {movieData})").WithParams(new { movieData = Migration1.Movie }).ExecuteWithoutResults();
        }

        public void Down(IGraphClient graphClient)
        {
            graphClient.Cypher.Match("(m:Movie)").Where((Movie m) => m.Title == Migration1.Movie.Title).Delete("m").ExecuteWithoutResults();
        }
    }
}
