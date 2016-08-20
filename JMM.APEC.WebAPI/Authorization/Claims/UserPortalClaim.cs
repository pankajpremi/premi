using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Authorization
{
    public class UserPortalClaim : ClaimValue
    {
        public int PortalId { get; set; }
        public int PortalPortalId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DomainUrl { get; set; }

        public UserPortalClaim(int portalid, int portalportalid, string name, string code, string domainurl)
        {
            this.PortalId = portalid;
            this.PortalPortalId = portalportalid;
            this.Name = name;
            this.Code = code;
            this.DomainUrl = domainurl;
        }

        public override string ValueType()
        {
            return "UserPortal";
        }
    }
}