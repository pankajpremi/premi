using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class GatewaySystemRoleDto
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int GatewayId { get; set; }
        public List<PermissionDto> Permissions { get; set; }

    }
}