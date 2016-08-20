using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class UserSystemRoleDto
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string GatewayCode { get; set; }
        public int GatewayId { get; set; }
        public int PortalGatewayId { get; set; }

    }
}