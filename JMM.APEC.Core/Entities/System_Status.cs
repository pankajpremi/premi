using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_Status
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int SortOrder { get; set; }

        // Foreign Key
        public int StatusTypeId { get; set; }
        // Navigation property
        public System_StatusType StatusType { get; set; }

       
    }
}
