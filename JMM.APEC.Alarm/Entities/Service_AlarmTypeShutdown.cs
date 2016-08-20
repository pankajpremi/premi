using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Alarm
{
    public class Service_AlarmTypeShutdown
    {
        
        public int AlarmTypeShutdownId { get; set; }
        public int EntityId { get; set; }
        public bool IsDeleted { get; set; }

        public int ObjectId { get; set; }

        public System_Object Obj { get; set; }

        public int AlarmCategoryId { get; set; }
            
        public Service_AlarmCategory AlarmCategory { get; set; }
    }
}
