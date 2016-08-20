using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Core;

namespace JMM.APEC.Alarm
{
    public class Service_AlarmWorklog
    {
        public int CommentId { get; set; }

        public int AlarmEventId { get; set; }

        public bool Haschild { get; set; }

        public int ParentId { get; set; }
        public string Comment { get; set; }

        public string EnteredBy { get; set; }

        public int EnteredByUserId { get; set; }

        public DateTime CommentDateTime { get; set; }

        public int? CommentParentId { get; set; }

        public int AppChangeUserId { get; set; }       

        public bool IsDeleted { get; set; }

        public int ObjectId { get; set; }
        public System_Object Obj { get; set; }

    }
}
