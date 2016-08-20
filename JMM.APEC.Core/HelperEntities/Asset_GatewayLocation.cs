using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
   public class Asset_GatewayLocation
    {
        public int GatewayId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        //public int? StateId { get; set; }
        //public int? CountryId { get; set; }

        public string StateCode { get; set; }
        public string CountryCode { get; set; }

        //contact info
        public string Email { get; set; }

        public List<Asset_PhoneInfo> Phones { get; set; }
        public int AppChangeUserId { get; set; }
    }
}
