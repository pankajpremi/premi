using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class FlowMeterDto
    {
        public int EventId { get; set; }
        public string FuelPositionNumber { get; set; }
        public string FuelPositionLabel { get; set; }
        public int FuelPositionId { get; set; }
        public string FlowMeterNumber { get; set; }
        public string HoseNumber { get; set; }
        public decimal AvgGPMValue { get; set; }
        public int NumOfTransactions { get; set; }
        public decimal TotalVolumeAmt { get; set; }
        public DateTime ReadingDateTime { get; set; }
    }
}