using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_Object
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        // Foreign Key
        public int CategoryId { get; set; }
        // Navigation property
        public System_Category Category { get; set; }
    }
}
