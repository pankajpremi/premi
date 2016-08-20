using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_SLA
    {
        public int SLAId { get; set; }

        public string SLAName { get; set; }

        public int ResponseMinutes { get; set; }

        public bool Active { get; set; }

        public int? SortOrder { get; set; }

        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }

        public int ObjectId { get; set; }
        public System_Object obj { get; set; }
    }
}
