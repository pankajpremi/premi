using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceActivityLogCommentDto
    {
        public int ActivityLogId { get; set; }

        public int CommentId { get; set; }

        public string Comment { get; set; }

        public string EnteredBy { get; set; }
       public DateTime CommentDateTime { get; set; }
    }
}