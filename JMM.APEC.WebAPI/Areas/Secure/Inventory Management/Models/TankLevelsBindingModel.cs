using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class TankLevelsBindingModel
    {
        public string Gateways { get; set; }
        public string Facilities { get; set; }
        public string Products { get; set; }
        public int? PercentLevel { get; set; }
    }
}