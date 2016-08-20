using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Contact
    {
        public int ContactId { get; set; }
        public int? TitleId { get; set; }
        public int? ObjectId { get; set; }
        public string ObjectCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool IsAutoAdd { get; set; }
        public bool IsDeleted { get; set; }
        public int AppChangeUserId { get; set; }
      
        public int? AddressId { get; set; }
        public Asset_Address Address { get; set; }

        public int? TypeId { get; set; }
        public System_Type Type { get; set; }

        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }

           
    }
}
