using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations.Tests
{
    class FakeAssembly : Assembly
    {
        public FakeAssembly(params Type[] types)
        {
            Types = types;
        }

        public Type[] Types { get; set; }

        public override Type[] GetExportedTypes()
        {
            return Types;
        }

        public override string FullName
        {
            get
            {
                return "FakeAssembly";
            }
        }
    }
}
