using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Neo4jClient.Migrations.Tests.MigratorTests
{
    public class Migrate_Exceptions : BaseTests
    {
        Migrator sut;

        public Migrate_Exceptions()
        {
            sut = new Migrator(graphClient);
        }

        [Fact]
        public void Should_Throw_Exception_When_Assembly_Does_Not_Have_Migrations() 
        {
            Assembly fakeAssembly = new FakeAssembly();
            string expectedMessage = string.Format("No migrations defined in assembly {0}.", fakeAssembly.FullName);

            var exception = Assert.Throws<ArgumentException>(() => sut.Migrate(fakeAssembly));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Should_Throw_Exception_When_Version_Does_Not_Exists()
        {
            long unexistingVersion = 3;
            string expectedMessage = string.Format("Migration {0} not defined. Omit the toVersion parameter to migrate to the latest version (actually: {1}).", unexistingVersion, 2);

            var exception = Assert.Throws<ArgumentException>(() => sut.Migrate(Assembly.GetExecutingAssembly(), unexistingVersion));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
