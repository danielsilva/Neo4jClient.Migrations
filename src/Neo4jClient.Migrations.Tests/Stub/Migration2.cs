using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations.Tests.Stub
{
    [Migration(2)]
    public class Migration2 : IMigration
    {
        public static Movie Movie 
        { 
            get 
            {
                return new Movie { Title = "The Matrix Reloaded" };
            } 
        }

        public void Up(IGraphClient graphClient)
        {
            graphClient.Cypher.Create("(m:Movie {movieData})").WithParams(new { movieData = Migration2.Movie }).ExecuteWithoutResults();
        }

        public void Down(IGraphClient graphClient)
        {
            graphClient.Cypher.Match("(m:Movie)").Where(string.Format("m.Title = '{0}'", Migration2.Movie.Title)).Delete("m").ExecuteWithoutResults();
        }
    }
}
