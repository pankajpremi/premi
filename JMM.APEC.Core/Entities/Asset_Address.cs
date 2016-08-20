using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Address
    {
        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CornerAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int AppChangeUserId { get; set; }
        public int? StateId { get; set; }
        public System_State State { get; set; }
        public int? CountyId { get; set; }
        public System_County County { get; set; }
        public int? CountryId { get; set; }
        public System_Country Country { get; set; }
        public int? TimeZoneId { get; set; }
        public System_TimeZone TimeZone { get; set; }
    }
}
