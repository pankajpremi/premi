using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceEventTrackerSubTypeDto
    {
        public int TypeId { get; set; }
        public int SubTypeId { get; set; }
        public string SubTypeName { get; set; }
    }
}