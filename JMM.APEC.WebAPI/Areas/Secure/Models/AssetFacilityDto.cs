using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
   public class AssetFacilityDto
    {

        public int GatewayId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAKA { get; set; }
        public int AddressId { get; set; }
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusValue { get; set; }
        public string StatusDesc { get; set; }
        public int TypeId { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public int CountyId { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public bool? Deleted { get; set; }
        public string PrimaryPhone { get; set; }
    }
}
