using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Entities
{
    public class System_MessageUserDismissal
    { 
        public int Id { get; set; }
        public int MessageId { get; set; }
        public System_Message Message { get; set; }
        public int UserId { get; set; }
    }
}
