using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    public class ApplicationGateway
    {
        public int Id { get; set; }
        public int? PortalGatewayId { get; set; }
        public int PortalId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ApplicationGateway()
        {

        }

        public ApplicationGateway(string name, int id, int? portalgatewayid, int portalid, string code)
        {
            Name = name;
            Id = id;
            PortalGatewayId = portalgatewayid;
            PortalId = portalid;
            Code = code;
        }

    }
}