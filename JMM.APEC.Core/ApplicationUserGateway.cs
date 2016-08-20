using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class ApplicationUserGateway
    {
        public int Id { get; set; }
        public int PortalGatewayId { get; set; }
        public int PortalId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
