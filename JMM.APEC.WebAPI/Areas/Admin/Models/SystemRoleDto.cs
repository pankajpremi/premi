using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class SystemRoleDto
    {
        public int SystemRoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ServiceCode { get; set; }
    }
}