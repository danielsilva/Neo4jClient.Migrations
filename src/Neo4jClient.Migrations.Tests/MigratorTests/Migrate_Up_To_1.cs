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
    public class Migrate_Up_To_1 : BaseTests
    {
        Migrator sut;

        public Migrate_Up_To_1()
        {
            sut = new Migrator();
            sut.Migrate(graphClient, Assembly.GetExecutingAssembly(), 1);
        }

        [Fact]
        public void Should_Create_Movie_From_Migration1() 
        {
            var existingMovie = graphClient.Cypher.Match("(m:Movie)").Return(m => m.As<Movie>()).Results.Single();
            Assert.Equal(Migration1.Movie.Title, existingMovie.Title);
        }

        [Fact]
        public void Should_Save_MigrationInfo() 
        {   
            var migrationInfo = graphClient.Cypher.Match("(m:MigrationInfo)").Return(m => m.As<MigrationInfo>()).Results.Single();
            Assert.NotEqual(Guid.Empty, migrationInfo.Id);
            Assert.Equal(1, migrationInfo.Version);
        }
    }
}
