using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_StatusGateway
    {
        public int StatusGatewayId { get; set; }
        public int StatusId { get; set; }
        public System_Status Status { get; set; }
        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }
        public string DescriptionOverride { get; set; }
        public bool IsDeleted { get; set; }
    }
}
