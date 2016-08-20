using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class RdReportResultDto
    {
        public int ReleaseDetectionId { get; set; }
        public string GatewayName { get; set; }
        public int GatewayId { get; set; }
        public string FacilityName { get; set; }
        public int FacilityId { get; set; }
        public int AtgId { get; set; }
        public string AtgName { get; set; }
        public RdAssetTemplateDto Template { get; set; }
        public RdAssetResultDto Result { get; set; }
        public RdAssetDto Asset { get; set; }
        public RdStatusDto RdStatus { get; set; }

    }
}