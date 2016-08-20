using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SensorAssetDto
    {
        public int SensorId { get; set; }
        public string Asset { get; set; }
        public string AtgName { get; set; }
        public string SensorLabel { get; set; }
    }
}