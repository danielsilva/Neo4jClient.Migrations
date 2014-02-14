using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations
{
    public class MigrationInfo
    {
        public Guid Id { get; set; }
        public long Version { get; set; }
        [JsonIgnore]
        public IMigration Migration { get; set; }

        public static MigrationInfo New(long version, Type migrationType)
        {
            return new MigrationInfo
            {
                Id = Guid.NewGuid(),
                Version = version,
                Migration = (IMigration)Activator.CreateInstance(migrationType)
            };
        }
    }
}
