using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceActivityLogDto
    {
        public int ActivityLogId { get; set; }

        public int? TypeId { get; set; }

        public string FacilityName { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string TypeName { get; set; }

        public string EnteredBy { get; set; }

        public DateTime? LogDateTime { get; set; }
        public int? UserId { get; set; }     

        public int GatewayId { get; set; }
        public string ObjectId { get; set; }

        public int NumOfComments { get; set; }

    }
}