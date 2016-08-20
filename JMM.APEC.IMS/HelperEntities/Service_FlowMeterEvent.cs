using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS
{
    public class Service_FlowMeterEvent
    {
        public DateTime ReadingDateTime { get; set; }
        public int NumOfTransactions { get; set; }
        public decimal AvgGPMValue { get; set; }
        public decimal TotalVolumeAmt { get; set; }
    }
}
