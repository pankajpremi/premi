using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ReleaseDetection
{
    public class Service_ReleaseDetectionTest
    {

        public int RdTestId { get; set; }

        public int EntityId { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public int? TestMonth { get; set; }

        public int? TestYear { get; set; }

        public string CalculatedLeakRate { get; set; }

        public bool? Override { get; set; }

        public DateTime? WaterCheckDate { get; set; }

        public string  WaterLevel { get; set; }

        public int ObjectId { get; set; }
        public System_Object Object { get; set; }

        public int StatusId { get; set; }
        public System_Status Status { get; set; }

        public int VendorId { get; set; }
        public Asset_Vendor Vendor { get; set; }
    }
}
