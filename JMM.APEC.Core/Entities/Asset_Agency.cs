using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Agency
    {
        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string Program { get; set; }
        public string Attention { get; set; }
        public string AgencyUrl { get; set; }
        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }
        public int AddressId { get; set; }
        public Asset_Address Address { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int AppChangeUserId { get; set; }
    }
}
