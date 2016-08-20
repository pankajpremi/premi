using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Alarm
{
   public class Service_AlarmInfo
    {
        public int AlarmEventId { get; set; }
        public int AlarmId { get; set; }

        public int GatewayId { get; set; }
       
        public string GatewayName { get; set; }

        public string FacilityName { get; set; }

        public string AlarmCode { get; set; }

        public string AlarmName { get; set; }

        public int SLAId { get; set; }

        public DateTime SLADue { get; set; }

        public string Status { get; set; }


    }
}
