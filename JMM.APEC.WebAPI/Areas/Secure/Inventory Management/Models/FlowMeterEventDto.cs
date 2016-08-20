using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class FlowMeterEventDto
    {
        public decimal AvgGPMValue { get; set; }
        public int NumOfTransactions { get; set; }
        public decimal TotalVolumeAmt { get; set; }
        public DateTime ReadingDateTime { get; set; }
    }
}