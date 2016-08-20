using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemMessageGatewayBindingModel
    {
        public int TypeId { get; set; }
        [Required]
        public string ObjectCode { get; set; }
        [Required]
        public string TypeCode { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public int FromUserId { get; set; }
        public bool IsDismissible { get; set; }
        [Required]
        public DateTime BeginDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public List<Asset_GatewayInfo> gatewaylist { get; set; }
    }
}