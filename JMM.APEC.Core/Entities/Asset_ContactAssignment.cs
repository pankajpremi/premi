using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_ContactAssignment
    {
        public int ContactAssignmentId { get; set; }
        public int ContactId { get; set; }
        public int ObjectId { get; set; }
        public int EntityId { get; set; }
        public bool IsDeleted { get; set; }
        public Asset_Contact Contact { get; set; }
        public System_Object Obj { get; set; }
    }
}
