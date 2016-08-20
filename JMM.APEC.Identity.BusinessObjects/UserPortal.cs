using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserPortal : BusinessObject
    {
        public int PortalId { get; set; }
        public int PortalPortalId { get; set; }
        public int UserId { get; set; }
        public string PortalName { get; set; }
        public string PortalCode { get; set; }
        public string ConnectionName { get; set; }
        public string DomainUrls { get; set; }
        public bool? IsActive { get; set; }

        public UserPortal(int portalId, int portalPortalId, int userId, string portalName, string portalCode, string domainUrls, bool isActive)
        {
            UserId = userId;
            PortalName = portalName;
            PortalId = portalId;
            PortalPortalId = portalPortalId;
            PortalCode = portalCode;
            DomainUrls = domainUrls;
            IsActive = isActive;
        }

        public UserPortal()
        {

        }

    }
}
