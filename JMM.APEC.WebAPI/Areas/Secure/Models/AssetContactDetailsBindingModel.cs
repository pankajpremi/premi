using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using JMM.APEC.Core;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetContactDetailsBindingModel
    {
         public int? TypeId { get; set; }
        [MaxLength(60)]
        public string Company { get; set; }
        public int? TitleId { get; set; }
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(25)]
        public string LastName { get; set; }
        [MaxLength(60)]
        public string Address1 { get; set; }
        [MaxLength(60)]
        public string Address2 { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        [MaxLength(60)]
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^[a-zA-Z0-9 ]{1,10}$", ErrorMessage = "Postal Code must be alphanumeric and no more than 10 characters in length")]
        public string PostalCode { get; set; }
        [DataType(DataType.EmailAddress)]
        [MaxLength(60)]
        public string Email { get; set; }
        public bool? IsAutoAdd { get; set; }
        public List<Asset_PhoneInfo> Phonelist { get; set; }
        public List<Asset_Facility> Facilitylist { get; set; }
    


    }
}