using JMM.APEC.WebAPI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemMessageGatewayDto
    {
        public int MessageId { get; set; }
        public List<AssetGatewayDto> Gatewaylist { get; set; }        

    }
}