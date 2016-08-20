using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserFacilityModel
    {
        public int FacilityId { get; set; }
        public int PortalGatewayId { get; set; }
        public string FacilityName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int PortalPortalId { get; set; }
        public bool Selected { get; set; }
        public int UserId { get; set; }
    }
}
