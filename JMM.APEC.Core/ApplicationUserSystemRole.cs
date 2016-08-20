using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class ApplicationUserSystemRole
    {
        public int GatewayId { get; set; }
        public string GatewayCode { get; set; }
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; }
        public int Portalid { get; set; }
        public string ServiceCode { get; set; }
    }
}
