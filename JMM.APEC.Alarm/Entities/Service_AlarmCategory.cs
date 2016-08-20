using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Alarm
{
    public class Service_AlarmCategory
    {

        public int DBId { get; set; }
               
        public int? AtgCategoryId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public bool Active { get; set; }

        public int? SortOrder { get; set; }

        public int? AtgBrandId { get; set; }
        public System_Brand Brand { get; set; }
    }
}
