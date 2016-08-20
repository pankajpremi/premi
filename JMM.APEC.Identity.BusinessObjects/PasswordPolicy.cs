using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class PasswordPolicy : BusinessObject
    {
        public int? Id { get; set; }
        public int? PortalId { get; set; }
        public int? RequiredLength { get; set; }
        public bool? RequireNonLetterOrDigit { get; set; }
        public bool? RequireDigit { get; set; }
        public bool? RequireLowercase { get; set; }
        public bool? RequireUppercase { get; set; }
    }
}
