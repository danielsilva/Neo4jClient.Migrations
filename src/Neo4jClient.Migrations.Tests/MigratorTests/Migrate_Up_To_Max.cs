using Neo4jClient.Migrations.Tests.Stub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Neo4jClient.Migrations.Tests.MigratorTests
{
    public class Migrate_Up_To_Max : BaseTests
    {
        Migrator sut;
        IList<Movie> results;

        public Migrate_Up_To_Max()
        {
            sut = new Migrator(graphClient);
            sut.Migrate(Assembly.GetExecutingAssembly());
            results = graphClient.Cypher.Match("(m:Movie)").Return(m => m.As<Movie>()).Results.ToList();
        }

        [Fact]
        public void Should_Create_Movie_From_Migration1() 
        {
            Assert.Equal(Migration1.Movie.Title, results[0].Title);
        }

        [Fact]
        public void Should_Create_Movie_From_Migration2()
        {
            Assert.Equal(Migration2.Movie.Title, results[1].Title);
        }

        [Fact]
        public void Should_Save_MigrationInfo() 
        {   
            var migrationInfos = graphClient.Cypher.Match("(m:MigrationInfo)").Return(m => m.As<MigrationInfo>()).Results.ToList();

            Assert.Equal(2, migrationInfos.Count());
            Assert.Equal(1, migrationInfos[0].Version);
            Assert.Equal(2, migrationInfos[1].Version);
        }
    }
}
