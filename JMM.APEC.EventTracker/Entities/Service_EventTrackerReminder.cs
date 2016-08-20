using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Core;

namespace JMM.APEC.EventTracker
{
    public class Service_EventTrackerReminder
    {
        public int EventTrackerReminderId { get; set; }
        public int ObjectId { get; set; }
        public int EntityId { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int SubTypeId { get; set; }
        public int ParentId { get; set; }
        public int CalendarCycleId { get; set; }       

        public DateTime DueDate { get; set; }

        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }
    }
}
