using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceEventTrackerTypeDto
    {
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}