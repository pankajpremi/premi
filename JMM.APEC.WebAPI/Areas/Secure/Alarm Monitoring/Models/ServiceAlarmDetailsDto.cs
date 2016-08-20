using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceAlarmDetailsDto
    {

        public int AlarmEventId { get; set; }
        public int AlarmId { get; set; }

        public int GatewayId { get; set; }

        public string AlarmCode { get; set; }

        public string AlarmName { get; set; }

        public int SLAId { get; set; }

        public DateTime SLADue { get; set; }

        public DateTime AlarmDateTime { get; set; }

        public int SensorId { get; set; }

        public string Sensor { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public string Asset { get; set; }
        public string Category { get; set; }
    }
}