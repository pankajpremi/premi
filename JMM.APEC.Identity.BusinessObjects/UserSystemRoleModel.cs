using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserSystemRoleModel
    {
        public int SystemRoleId { get; set; }
        public string SystemRoleName { get; set; }
        public string SystemRoleCode { get; set; }
        public bool Selected { get; set; }
        public int UserId { get; set; }
        public List<UserPermissionModel> Permissions { get; set; }
    }
}
