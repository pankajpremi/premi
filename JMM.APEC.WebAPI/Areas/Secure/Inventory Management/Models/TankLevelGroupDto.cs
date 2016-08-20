using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class TankLevelGroupDto
    {
        public string GatewayName { get; set; }
        public int GatewayId { get; set; }
        public string FacilityName { get; set; }
        public int FacilityId { get; set; }
        public int NumOfTankComparments { get; set; }
        public decimal MinLevelPercent { get; set; }
        public decimal AvgLevelPercent { get; set; }
        public decimal AvgWater { get; set; }
        public decimal MaxWater { get; set; }
    }
}