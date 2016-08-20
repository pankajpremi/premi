using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class RdAssetResultDto
    {
        public DateTime? ReportDate { get; set; }
        public string ResultName { get; set; }
        public string ResultColor { get; set; }
        public int ResultId { get; set; }
        public DateTime? TestDate { get; set; }
        public string TestDateTimeZone { get; set; }
        public string TestTypeLabel { get; set; }
        public int TestTypeId { get; set; }
        public string TestResultName { get; set; }
        public int TestResultId { get; set; }
        public string TestResultColor { get; set; }


    }
}