using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserClaim : BusinessObject
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

    }
}
