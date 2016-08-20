using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS
{
    public class Service_EventCompartmentDelivery
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
        public DateTime DeliveredDateTime { get; set; }
        public decimal DeliveredVolAmt { get; set; }
        public decimal DeliveredMeasAmt { get; set; }


    }
}
