using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserRole : BusinessObject
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
