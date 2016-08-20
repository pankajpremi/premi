using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceAlarmListDto
    {
        public int AlarmEventId { get; set; }

        public string GatewayName { get; set; }

        public string FacilityName { get; set; }

        public int FacilityId { get; set; }
        public int gatewayId { get; set; }

        public string JMMStatus { get; set; }

        public string AlarmType { get; set; }

        public string AlarmCategory { get; set; }

        public string Category { get; set; }

        public string Sensor { get; set; }
        public int SensorId { get; set; }

        public int ActiveAlarmsCount { get; set; }

        public string Duration { get; set; }

        
    }
}