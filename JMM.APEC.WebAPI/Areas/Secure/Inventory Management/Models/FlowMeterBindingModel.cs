using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class FlowMeterBindingModel
    {
        public string Gateways { get; set; }
        public string Facilities { get; set; }
        public decimal? GPMLevel { get; set; }
    }
}