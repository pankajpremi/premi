using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserGateway : BusinessObject
    {
        public string GatewayName { get; set; }
        public int UserId { get; set; }
        public int GatewayId { get; set; }
        public int? PortalGatewayId { get; set; }
        public int PortalId { get; set; }
        public string GatewayCode { get; set; }

        public UserGateway(string gatewayName, int gatewayId, int? portalGatewayId, string gatewayCode, int portalId, int userId)
        {
            GatewayName = gatewayName;
            GatewayId = gatewayId;
            PortalGatewayId = portalGatewayId;
            PortalId = portalId;
            GatewayCode = gatewayCode;
        }

        public UserGateway()
        {

        }
    }
}
