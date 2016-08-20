using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ReleaseDetection
{
    public class Service_ReleaseDetectionAssetTemplate
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool Primary { get; set; }
        public string MethodName { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
