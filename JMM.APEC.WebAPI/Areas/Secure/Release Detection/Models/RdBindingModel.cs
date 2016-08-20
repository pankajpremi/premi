using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class RdBindingModel
    {
        public string Gateways { get; set; }
        public string Facilities { get; set; }
        public int? ReleaseDetectionId { get; set; }
        public string Results { get; set; }
        public string RdStatuses { get; set; }
        public string AssetTypes { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}