using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Calendar
{ 
   public  class Service_CalendarToDo
    {
        public int EventTrackerReminderId { get; set; }
        public string GatewayName { get; set; }
        public string ServiceName { get; set; }
        public string FacilityName { get; set; }
        public string SubjectName { get; set; }
        public string Status { get; set; }

        public DateTime Duedate { get; set; }

    }
}
