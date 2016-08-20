using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Alarm
{
    public class Service_AlarmList
    {
        public int AlarmEventId { get; set; }

        public string GatewayName { get; set; }

        public string FacilityName { get; set; }

        public int FacilityId { get; set; }
        public int gatewayId { get; set; }

        public string JMMStatus { get; set; }

        public string AlarmType { get; set; }

        public string AlarmCategory { get; set; }

        public string Sensor { get; set; }
        public int SensorId { get; set; }

        public int ActiveAlarmsCount { get; set; }

        public double DurationInSeconds { get; set; }

        public DateTime AlarmDateTime { get; set; }
    }
}
