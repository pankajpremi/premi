using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class ApplicationSystemUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsValid { get; set; }
        public List<string> IdentityRoles { get; set; }
        public ApplicationUserPortal Portal { get; set; }
        public List<ApplicationUserGateway> Gateways { get; set; }
        public List<ApplicationUserSystemRole> SystemRoles { get; set; }
        public bool PortalAllowed { get; set; }

        public ApplicationSystemUser()
        {
            IsValid = false;
            IsAdmin = false;
            IsSuperAdmin = false;
        }

    }
}
