using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
   public class Asset_GatewayFacilityList
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string Status { get; set; }
        public int? AddressId { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public int CountyId { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
    }
}
