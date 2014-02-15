using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations.Tests
{
    public class FakeAssembly : Assembly
    {
        public FakeAssembly()
        {
            Types = new Type[]{};
        }

        public override string FullName { get { return "FakeAssembly"; } }
        public Type[] Types { get; set; }

        public override Type[] GetExportedTypes()
        {
            return Types;
        }
    }
}
