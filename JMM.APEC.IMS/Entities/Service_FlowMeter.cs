using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS
{
    public class Service_FlowMeter
    {
        public int EventId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string GatewayName { get; set; }
        public int GatewayId { get; set; }
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
