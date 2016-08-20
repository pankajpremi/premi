using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceCalendarToDoDto
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