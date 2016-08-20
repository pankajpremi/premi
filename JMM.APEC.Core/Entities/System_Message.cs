using JMM.APEC.Core.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
   public class System_Message : BusinessObject
    {

        public System_Message()
        {
            // establish business rules

            AddRule(new ValidateId("MessageId"));
            
        }

        public int MessageId { get; set; }

        public int? UserToId { get; set; }
        public int UserFromId { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsDismissible { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeId { get; set; }
        public string ObjectCode { get; set; }
        public string TypeCode { get; set; }
        public System_Type Type { get; set; }
        
    }
}
