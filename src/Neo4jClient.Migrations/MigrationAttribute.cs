using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(long version)
        {
            if (version < 0) 
                throw new ArgumentOutOfRangeException("version", "Migration version must be greater than zero.");

            Version = version;
        }

        public long Version { get; private set; }
    }
}
