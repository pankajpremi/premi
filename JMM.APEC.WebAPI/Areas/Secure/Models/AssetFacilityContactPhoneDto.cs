using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetFacilityContactPhoneDto
    {
        public int FacilityId { get; set; }
        public int ContactId { get; set; }
        public int PhoneId { get; set; }

        public int PhoneTypeId { get; set; }
            
        public string  ContactName { get; set; }

        public string ContactTitle { get; set; }
        public string ContactCompany { get; set; }

        public string PhoneNumber { get; set; }
        public string PhoneType  { get; set; }

        public string FacilityName { get; set; }

    }
}