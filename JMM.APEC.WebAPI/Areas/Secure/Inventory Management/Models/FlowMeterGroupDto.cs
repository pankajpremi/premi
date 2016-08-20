using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class FlowMeterGroupDto
    {
        public string GatewayName { get; set; }
        public int GatewayId { get; set; }
        public string FacilityName { get; set; }
        public int FacilityId { get; set; }
        public int NumOfDispensers { get; set; }
        public int NumOfTransactions { get; set; }
        public decimal MinGPM { get; set; }
        public decimal AvgGPM { get; set; }
    }
}