using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetGatewayLocationDto
    {
       public int GatewayId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }

        //contact info
        public string Email { get; set; }
        public List<Asset_Phone> Phones { get; set; }
    }
}