using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ReleaseDetection
{ 
    public class Service_ReleaseDetectionAsset
    {
        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public int AssetId { get; set; }
        public string AssetLabel { get; set; }
        public string AssetProperty1Label { get; set; }
        public string AssetProperty1Value { get; set; }
        public string AssetProperty2Label { get; set; }
        public string AssetProperty2Value { get; set; }
    }
}
