using JMM.APEC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetGatewayContactDto
    {
        public int GatewayId { get; set; }
        public List<AssetContactDto> Contactlist { get; set; }
        public int AppChangeUserId { get; set; }
    }
}