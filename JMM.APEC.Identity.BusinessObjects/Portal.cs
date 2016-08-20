using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class Portal : BusinessObject
    {
        public int ID { get; set; }
        public int PortalPortalID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DomainUrls { get; set; }
        public string DatabaseName { get; set; }
        public bool? IsActive { get; set; }

    }
}
