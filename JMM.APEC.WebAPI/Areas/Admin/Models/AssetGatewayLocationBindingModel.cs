using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class AssetGatewayLocationBindingModel
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        //public int? StateId { get; set; }
        //public int? CountryId { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public List<Asset_PhoneInfo> phones { get; set; }
        public int AppChangeUserId { get; set; }
    }
}