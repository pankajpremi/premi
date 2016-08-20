using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class UserSystemRoleModelDto
    {
        public int SystemRoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Selected { get; set; }
        public int UserId { get; set; }
        public List<UserPermissionModelDto> Permissions { get; set; }
    }
}