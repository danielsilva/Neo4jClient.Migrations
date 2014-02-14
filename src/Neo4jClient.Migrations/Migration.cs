using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClient.Migrations
{
    public class Migration : IMigration
    {
        public virtual void Up(IGraphClient graphClient)
        {
        }

        public virtual void Down(IGraphClient graphClient)
        {
        }
    }
}
