using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{ 
    public class System_SubType
    {
        public int SubTypeId { get; set; }
        public string SubTypeCode { get; set; }
        public string SubTypeName { get; set; }
        public bool Active { get; set; }
       
        // Foreign Key
        public int TypeId { get; set; }
        // Navigation property
        public System_Type Type { get; set; }
    }
}
