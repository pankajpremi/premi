using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemTypeBindingModel
    {
        public int? ObjectId { get; set; }
        public string ObjectCode { get; set; }
        public string GatewayIds { get; set; }
        public string TypeCode { get; set; }
    }
}