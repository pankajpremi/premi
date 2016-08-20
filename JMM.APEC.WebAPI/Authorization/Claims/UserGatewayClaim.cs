using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Authorization
{
    public class UserGatewayClaim : ClaimValue
    {

        public int GatewayId { get; set; }
        public int? PortalGatewayId { get; set; }
        public int PortalId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public UserGatewayClaim(int id, int? portalgatewayid, int portalid, string name, string code)
        {
            this.GatewayId = id;
            this.PortalGatewayId = portalgatewayid;
            this.PortalId = portalid;
            this.Name = name;
            this.Code = code;
        }

        public override string ValueType()
        {
            return "UserGateway";
        }

    }
}