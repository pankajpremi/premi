using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class RdAssetTemplateDto
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool Primary { get; set; }
        public string MethodName { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}