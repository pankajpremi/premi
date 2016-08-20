using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceActivityLogTypeDto
    {
        public int TypeId { get; set; }
        public int GatewayId { get; set; }
        public string TypeCode { get; set; }

        public string TypeName { get; set; }
    }
}