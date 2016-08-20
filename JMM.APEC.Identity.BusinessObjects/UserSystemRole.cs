using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserSystemRole : BusinessObject
    {

        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public int RoleId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public int PermissionId { get; set; }
        public int UserId { get; set; }
        public int GatewayId { get; set; }
        public string GatewayName { get; set; }
        public string GatewayCode { get; set; }
        public string ServiceCode { get; set; }
        public int PortalId { get; set; }
        public bool Selected { get; set; }

        public UserSystemRole()
        {

        }

    }
}
