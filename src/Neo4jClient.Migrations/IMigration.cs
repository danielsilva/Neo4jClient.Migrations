using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations
{
    public interface IMigration
    {
        void Up(IGraphClient session);
        void Down(IGraphClient session);
    }
}
