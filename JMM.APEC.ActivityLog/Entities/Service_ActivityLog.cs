using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActivityLog
{
    public class Service_ActivityLog
    {
        public int ActivityLogId { get; set; }

        public int EntityId { get; set; }

        public string FacilityName { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
        public DateTime? LogDate { get; set; }
        public string EnteredBy { get; set; }

        public int Totalcomments { get; set; }
        public int? UserId { get; set; }
        public bool IsDeleted { get; set; }

        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }

        public int? ObjectId { get; set; }
        public System_Object obj { get; set; }

        public int? TypeId { get; set; }
        public System_Type Type { get; set; }
    }
}
