using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Phone
    {

        public int PhoneId { get; set; }
        public int GatewayId { get; set; }
        public string Number { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeId { get; set; }
        public System_Type Type { get; set; }
        public int AppChangeUserId { get; set; }
        public Asset_Gateway Gateway { get; set; }
    }
}
