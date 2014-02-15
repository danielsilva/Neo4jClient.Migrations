using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient.Migrations.Tests.Stub;
using Xunit;

namespace Neo4jClient.Migrations.Tests.MigratorTests
{
    public class Migrate_Down_From_2 : BaseTests
    {
        Migrator sut;

        public Migrate_Down_From_2()
        {
            sut = new Migrator(graphClient);
            sut.Migrate(Assembly.GetExecutingAssembly());

            sut.Migrate(Assembly.GetExecutingAssembly(), 1);
        }

        [Fact]
        public void Should_Have_Only_The_Movie_From_Migration1() 
        {
            var movie = graphClient.Cypher.Match("(m:Movie)").Return(m => m.As<Movie>()).Results.Single();
            Assert.Equal(Migration1.Movie.Title, movie.Title);
        }

        [Fact]
        public void Should_Have_Only_The_MigrationInfo_From_Migration1()
        {
            var info = graphClient.Cypher.Match("(m:MigrationInfo)").Return(m => m.As<MigrationInfo>()).Results.Single();
            Assert.Equal(1, info.Version);
        }
    }
}
