using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemMessageDto
    {
        public int MessageId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsDismissible { get; set; }
        public string Status { get; set; }
        public int GatewayCount { get; set; }
        public bool IsDeleted { get; set; }

    }
}