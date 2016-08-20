using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class GatewaySystemRole : BusinessObject
    {
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public int RoleId { get; set; }
        public int GatewayId { get; set; }
        public int GatewayPortalId { get; set; }
        public string GatewayName { get; set; }
        public string GatewayCode { get; set; }
        public string ServiceCode { get; set; }
        public int PortalId { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
