using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_StatusType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string ChangeUser { get; set; }
    }
}
