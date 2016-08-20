using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetPortalDto
    {
        public int PortalId { get; set; }
        public string PortalName { get; set; }
        public string DomainUrl { get; set; }
        public bool Active { get; set; }
        
    }
}
