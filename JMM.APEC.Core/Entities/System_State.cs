using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }
        public int CountryId { get; set; }
        public System_Country Country { get; set; }


    }
}
