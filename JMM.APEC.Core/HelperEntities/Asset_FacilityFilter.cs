using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_FacilityFilter
    {
        public int? GatewayId { get; set; }
        public string GatewayName { get; set; }
       public List<Asset_Facility> facilities { get; set; }
    }
}
