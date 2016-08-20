using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemStatusDto
    {
        public int GatewayId { get; set; }
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusValue { get; set; }
        public string Description { get; set; }
        public string StatusTypeCode { get; set; }
        public string StatusTypeName { get; set; }
    }
}