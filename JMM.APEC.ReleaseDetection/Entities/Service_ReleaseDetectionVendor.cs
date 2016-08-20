using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ReleaseDetection
{
    public class Service_ReleaseDetectionVendor
    {

        public int RDVendorId { get; set; }

        public int FacilityId { get; set; }
        public Asset_Facility Facility { get; set; }

        public int VendorId { get; set; }
        public Asset_Vendor Vendor { get; set; }
    }
}
