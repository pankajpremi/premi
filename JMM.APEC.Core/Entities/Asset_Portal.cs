using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Portal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DomainUrl { get; set; }
        public bool Active { get; set; }
        public int ModifiedUserId { get; set; }    


    }
}
