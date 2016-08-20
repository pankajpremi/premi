using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_County
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }     
        public int StateId { get; set; }
        public System_State State { get; set; }
    }
}
