using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceActivityLogBindingModel
    {
        public List<string> SortFields { get; set; }
        public List<string> SortDirections { get; set; }
        public string Gateways { get; set; }
        public string Facilities { get; set; }
        public string AlTypes { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}