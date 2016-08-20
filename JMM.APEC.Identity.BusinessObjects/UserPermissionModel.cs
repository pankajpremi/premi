using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserPermissionModel
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public bool Selected { get; set; }
        public int UserId { get; set; }
    }
}
