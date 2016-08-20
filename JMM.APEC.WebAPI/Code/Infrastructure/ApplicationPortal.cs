using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    public class ApplicationPortal
    {
        public int Id { get; set; }
        public int PortalPortalId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<string> DomainUrls { get; set; }
        public string CurrentDomain { get; set; }
        public bool CurrentPortal { get; set; }

        public ApplicationPortal()
        {

        }

        public ApplicationPortal(string name, int id, string code, string domainurls)
        {
            Name = name;
            Id = id;
            Code = code;
            DomainUrls = domainurls.Split(',').ToList();
        }


    }
}