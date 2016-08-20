using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}