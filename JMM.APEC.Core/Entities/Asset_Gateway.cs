using JMM.APEC.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Gateway: BusinessObject
    {
        public Asset_Gateway()
        {
            // establish business rules

            AddRule(new ValidateId("Id"));

        }
        public int Id { get; set; }
        public int IdentityId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int AppChangeUserId { get; set; }

        // Foreign Key
        public int PortalId { get; set; }
        // Navigation property
        public Asset_Portal Portal { get; set; }

        // Foreign Key
        public int? StatusId { get; set; }
        // Navigation property
        public System_Status Status { get; set; }

        // Foreign Key
        public int? AddressId { get; set; }
        // Navigation property
        public Asset_Address Address { get; set; }
    }
}
