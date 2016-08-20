using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class StatusDto
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}