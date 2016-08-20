using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceAlarmBindingModel
    {
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public string Gateways { get; set; }
        public string Facilities { get; set; }
        public string Statuses { get; set; }
        public string slas { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchText { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }
}