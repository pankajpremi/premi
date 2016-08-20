using JMM.APEC.Identity.BusinessObjects.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class User : BusinessObject
    {
        public User()
        {
            // establish business rules
            AddRule(new ValidateId("Id"));
            AddRule(new ValidateRequired("Email"));
            AddRule(new ValidateEmail("Email"));

        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool Approved { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime SignUpDate { get; set; }
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public DateTime StatusUpdateDateTime { get; set; }
        public List<UserPortal> UserPortals { get; set; }
        public List<UserGateway> UserGateways { get; set; }
        public int GatewayCount { get; set; }
        public UserProfile Profile { get; set; }
        public DateTime LastLoginDate { get; set; }

    }
}
