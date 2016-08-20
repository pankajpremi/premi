using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetFacilityMapDto
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
       
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}