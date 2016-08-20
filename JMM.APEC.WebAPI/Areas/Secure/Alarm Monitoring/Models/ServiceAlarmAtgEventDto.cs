using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceAlarmAtgEventDto
    {
        public string Type { get; set; }

        public DateTime Time { get; set; }

        public string Event { get; set; }

        public string Information { get; set; }
    }
}