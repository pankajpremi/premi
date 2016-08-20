using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemStatusBindingModel
    {
        public string SortFields { get; set; }
        public string SortDirections { get; set; }
        public string Gateways { get; set; }
        public string  StatusCode { get; set; }

    }
}