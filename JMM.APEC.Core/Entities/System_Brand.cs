using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_Brand
    {
        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public bool Active { get; set; }

        public int ObjectId { get; set; }
        public System_Object Obj { get; set; }

        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }

        public int TypeId { get; set; }
        public System_Type Type { get; set; }
    }
}
