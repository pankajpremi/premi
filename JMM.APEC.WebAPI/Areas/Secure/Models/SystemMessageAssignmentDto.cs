using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemMessageAssignmentDto
    {
      public int MessageId { get; set; }
      public List<Asset_GatewayInfo> GatewayList { get; set; }
    }
}