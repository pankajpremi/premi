using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS
{
    public class Service_TankCompartmentLevelGroup
    {
        public int GatewayId { get; set; }
        public string GatewayName { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public decimal NumOfTanks { get; set; }
        public decimal MinLevelPercent { get; set; }
        public decimal AvgLevelPercent { get; set; }
        public decimal MaxWater { get; set; }
        public decimal AvgWater { get; set; }
    }
}
