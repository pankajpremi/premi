using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActivityLog
{
    public class Service_ActivityLogComment
    {
        public int CommentId { get; set; }
        public int ActivityLogId { get; set; }
        public bool Haschild { get; set; }
        public string Comment { get; set; }
        public string EnteredBy { get; set; }
        public int EnteredByUserId { get; set; }
        public DateTime CommentDateTime { get; set; }
        public int? CommentParentId { get; set; }
        public int AppChangeUserId { get; set; }
        //public List<Service_ActivityLogComment> ChildComment { get; set; }
        public bool IsDeleted { get; set; }
        public int ObjectId { get; set; }
        public System_Object Obj { get; set; }
    }
}
