using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_PhoneAssignment
    {
        public int Id { get; set; }
        public int PhoneId { get; set; }
        public int ObjectId { get; set; }
        public int EntityId { get; set; }
        public bool IsDeleted { get; set; }
        public Asset_Phone Phone { get; set; }
        public System_Object Obj { get; set; }
    }
}
