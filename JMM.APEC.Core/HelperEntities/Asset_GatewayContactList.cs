using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
   public class Asset_GatewayContactList
    {
        public int ContactId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool IsAutoAdd { get; set; }
        public int AppChangeUserId { get; set; }
        public string Phone { get; set; }
        public int? AddressId { get; set; }     
        public int? TypeId { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public int GatewayId { get; set; }
      

    }
}
