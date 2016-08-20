using JMM.APEC.Alarm;
using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
   public class Service_Alarm
    {

        public int AlarmId { get; set; }
                      
        public int? AtgTypeId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }

        public int? SortOrder { get; set; }

        public int AppUserChangeId { get; set; }

        public int AlarmCategoryId { get; set; }
        public Service_AlarmCategory AlarmCategory { get; set; }

        public int SLAId { get; set; }
        public System_SLA SLA { get; set; }

    }
}
