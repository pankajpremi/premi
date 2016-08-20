using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS
{
    public class Service_EventCompartmentLevel
    {
        public int EventId { get; set; }
        public int GatewayId { get; set; }
        public string GatewayName { get; set; }
        public int FacilityId { get; set; }
        public Asset_GatewayFacility Facility { get; set; }
        public string FacilityName { get; set; }
        public int AtgId { get; set; }
        public int TankCompartmentId { get; set; }
        public int TankCompartmentNumber { get; set; }
        public string TankCompartmentLabel { get; set; }
        public string ProductLabel { get; set; }
        public DateTime? ReadingDateTime { get; set; }
        public int ReadingDateInt { get; set; }
        public int ReadingTimeInt { get; set; }
        public decimal HeightAmt { get; set; }
        public decimal UllageAmt { get; set; }
        public decimal VolumeAmt { get; set; }
        public decimal WaterHeightAmt { get; set; }
        public decimal CapacityAmt { get; set; }
        public decimal PercentVolumeAmt { get; set; }
        public Service_EventCompartmentDelivery LatestDelivery { get; set; }
    }
}
