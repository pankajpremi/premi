using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class UserFacilityModelDto
    {
        public int FacilityId { get; set; }
        public int GatewayId { get; set; }
        public int PortalId { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public bool Selected { get; set; }
        public int UserId { get; set; }
    }
}