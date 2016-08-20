using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class TankLevelDto
    {
        public int GatewayId { get; set; }
        public int FacilityId { get; set; }
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
        public DateTime? EstEmpty { get; set; }
        public TankDeliveryDto LatestDelivery { get; set; }

    }
}