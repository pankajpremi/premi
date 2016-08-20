using JMM.APEC.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
   public class Asset_ContactInformation : BusinessObject
    {

        public Asset_ContactInformation()
        {
            // establish business rules
            AddRule(new ValidateId("ContactId"));
            AddRule(new ValidateId("AddressId"));
        }
        public int? ContactId { get; set; }
        public int? AddressId { get; set; }
        public int GatewayId { get; set; }
        public string ObjectCode { get; set; }
        public int? TypeId { get; set; }
        public string Company { get; set; }
        public int? TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }       
        public string Email { get; set; }
        public bool? IsAutoAdd { get; set; }
        public List<Asset_PhoneInfo> Phonelist { get; set; }
        public List<Asset_Facility> Facilitylist { get; set; }

    }
}
