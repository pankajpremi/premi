using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetAddressDto
    {
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CornerAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public int TimeZoneId { get; set; }
        public string TimeZoneCode { get; set; }

        public string TimeZoneName { get; set; }

        public int TimeZoneOffset { set; get; }

        public string TimeZoneGMT { set; get; }

        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public string CountyCode { get; set; }

    }
}