using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations
{
    public class Migrator
    {
        private readonly IGraphClient graphClient;

        public Migrator(IGraphClient graphClient)
        {
            this.graphClient = graphClient;
        }

        public void Migrate(Assembly assemblyWithMigrations, long toVersion = -1)
        {
            bool toMax = toVersion < 0;
            var migrations = GetMigrations(assemblyWithMigrations);
            EnsureCanMigrate(toVersion, migrations, assemblyWithMigrations);

            if (toMax) toVersion = long.MaxValue;

            var appliedMigrations = GetAppliedMigrations();
            var appliedVersions = new HashSet<long>(appliedMigrations.Select(m => m.Version));
            var currentVersion = appliedVersions.Count == 0 ? 0 : appliedVersions.Max();

            if (toVersion > currentVersion)
            {
                MigrateUpTo(toVersion, appliedVersions, migrations);
            }
            else
            {
                MigrateDownTo(toVersion, appliedMigrations, appliedVersions, migrations);
            }
        }

        private Dictionary<long, Type> GetMigrations(Assembly assemblyWithMigrations)
        {
            var migrationMetadata = 
                from type in assemblyWithMigrations.GetExportedTypes()
                where typeof(IMigration).IsAssignableFrom(type) && !type.IsAbstract && type.IsClass
                let attr = (MigrationAttribute)type.GetCustomAttributes(typeof(MigrationAttribute), false).FirstOrDefault()
                select new { type, attr };

            foreach (var metadata in migrationMetadata)
            {
                if (metadata.attr == null)
                    throw new InvalidOperationException(string.Format("The Migration attribute for migration {0} is missing", metadata.type.FullName));
            }

            return migrationMetadata.ToDictionary(m => m.attr.Version, m => m.type);
        }

        private void EnsureCanMigrate(long toVersion, Dictionary<long, Type> migrations, Assembly assemblyWithMigrations)
        {
            if (migrations.Count == 0)
                throw new ArgumentException("No migrations defined in assembly " + assemblyWithMigrations.FullName + ".");

            if (toVersion > 0 && !migrations.ContainsKey(toVersion))
                throw new ArgumentException(string.Format("Migration {0} not defined. Omit the toVersion parameter to migrate to the latest version (actually: {1}).", toVersion, migrations.Max(t => t.Key)));
        }

        private IEnumerable<MigrationInfo> GetAppliedMigrations()
        {
            return graphClient.Cypher.Match("(m:MigrationInfo)").Return(m => m.As<MigrationInfo>()).Results;
        }

        private void MigrateUpTo(long toVersion, ISet<long> appliedVersions, IDictionary<long, Type> migrations)
        {
            var migrationsToRun = migrations
                .Where(m => !appliedVersions.Contains(m.Key) && m.Key <= toVersion)
                .OrderBy(m => m.Key)
                .Select(m => MigrationInfo.New(m.Key, m.Value));

            foreach (var migrationInfo in migrationsToRun)
            {
                migrationInfo.Migration.Up(graphClient);
            }
            foreach (var migrationInfo in migrationsToRun)
            {
                graphClient.Cypher
                    .Create("(m:MigrationInfo {migrationInfo})")
                    .WithParams(new { migrationInfo = migrationInfo })
                    .ExecuteWithoutResults();
            }
        }

        private void MigrateDownTo(long toVersion, IEnumerable<MigrationInfo> appliedMigrations, ISet<long> appliedVersions, IDictionary<long, Type> migrations)
        {
            var migrationsToRun = appliedMigrations
                .Where(m => appliedVersions.Contains(m.Version) && m.Version > toVersion)
                .OrderByDescending(m => m.Version)
                .ToArray();
         
            foreach (var migrationInfo in migrationsToRun)
            {
                var migration = (IMigration)Activator.CreateInstance(migrations[migrationInfo.Version]);
                migration.Down(graphClient);
            }

            foreach (var migrationInfo in migrationsToRun)
            {
                graphClient.Cypher
                    .Match("(m:MigrationInfo)")
                    .Where((MigrationInfo m) => m.Id == migrationInfo.Id)
                    .Delete("m")
                    .ExecuteWithoutResults();
            }
        }
    }
}
