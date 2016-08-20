using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Authorization
{
    public class UserSystemRoleClaim : ClaimValue
    {
        public int GatewayId { get; set; }
        public string GatewayCode { get; set; }
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; }
        public int PortalId { get; set; }
        public string ServiceCode { get; set; }

        public UserSystemRoleClaim(int gatewayid, int roleid, int permissionid, string gatewaycode, string rolecode, string permissioncode, string servicecode, int portalid)
        {
            this.RoleCode = rolecode;
            this.RoleId = roleid;
            this.PermissionCode = permissioncode;
            this.PermissionId = permissionid;
            this.GatewayCode = gatewaycode;
            this.GatewayId = gatewayid;
            this.ServiceCode = servicecode;
            this.PortalId = portalid;

        }

        public override string ValueType()
        {
            return "UserSystemRole";
        }
    }
}