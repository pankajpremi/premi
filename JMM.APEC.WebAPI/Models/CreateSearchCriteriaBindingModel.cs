
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class CreateSearchCriteriaBindingModel
    {
        public string GatewayId { get; set; }
        public string FacilityId { get; set; }
        public string StatusId { get; set; }
        public string SlaId { get; set; }
        public string Result { get; set; }
        public string RdStatus { get; set; }
        public string Asset { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
    }



}