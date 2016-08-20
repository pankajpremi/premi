using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.EventTracker
{
    public class Service_EventTrackerAction
    {

        public int EventTrackerActionId {get; set;}

        public DateTime? ScheduledDateTime { get; set; }

        public int? VendorId { get; set; }
        public DateTime? CompletedDate { get; set; }

        public int? ResultStatusId { get; set; }

        public string CompletedBy { get; set; }

        public int EventTrackerReminderId { get; set; }
        public Service_EventTrackerReminder EventTrackerReminder { get; set; }


    }
}
