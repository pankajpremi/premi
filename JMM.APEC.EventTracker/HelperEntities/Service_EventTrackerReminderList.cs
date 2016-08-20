using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.EventTracker
{
   public class Service_EventTrackerReminderList
    {
        public int EventTrackerReminderId { get; set; }
        public int ObjectId { get; set; }
        public int GatewayId { get; set; }
        public int FacilityId { get; set; }
        public string GatewayName { get; set; }
        public string FacilityName { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int SubTypeId { get; set; }
        public int ParentId { get; set; }
        public int CalendarCycleId { get; set; }
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public string SubTypeName { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime DateCompleted { get; set; }
        public string Status { get; set; }

    }
}
