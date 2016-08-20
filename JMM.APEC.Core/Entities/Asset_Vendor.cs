using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int AppChangeUserId { get; set; }

        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }

        public int AddressId { get; set; }
        public Asset_Address Address { get; set; }
    }
}
