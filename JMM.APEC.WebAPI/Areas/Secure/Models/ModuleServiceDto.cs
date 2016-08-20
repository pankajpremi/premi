using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class ModuleDto
    {
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public List<ServiceDto> Services { get; set; }

    }

    public class ServiceDto
    {
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
    }

}