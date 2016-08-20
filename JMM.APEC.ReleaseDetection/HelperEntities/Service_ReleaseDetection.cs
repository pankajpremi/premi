using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ReleaseDetection
{
    public class Service_ReleaseDetection
    {
        public int ReleaseDetectionId { get; set; }
        public string GatewayName { get; set; }
        public int GatewayId { get; set; }
        public string FacilityName { get; set; }
        public int FacilityId { get; set; }
        public int AtgId { get; set; }
        public string AtgName { get; set; }
        //template information
        public Service_ReleaseDetectionAssetTemplate Template { get; set; }
        //result information
        public Service_ReleaseDetectionResult Result { get; set; }
        //asset information
        public Service_ReleaseDetectionAsset Asset { get; set; }
        //release detection status
        public Service_ReleaseDetectionStatus RdStatus { get; set; }
    }
}
