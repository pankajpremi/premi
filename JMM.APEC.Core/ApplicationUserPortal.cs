using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class ApplicationUserPortal
    {
        public int PortalId { get; set; }
        public int LocalPortalId { get; set; }
        public string ConnectionName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DomainUrl { get; set; }

    }
}
