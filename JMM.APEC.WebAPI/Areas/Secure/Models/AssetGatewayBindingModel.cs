using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetGatewayBindingModel
    {
        public int? PortalId { get; set; }
        public bool? PortalIsActive { get; set; }
        public int? GatewayId { get; set; }
        public string Gatewaycode { get; set; }
        public int? GatewayStatusCode { get; set; }
        
    }
}