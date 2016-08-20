using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemMessageBindingModel
    {
        public int? MessageId { get; set; }
        public string TypeId { get; set; }
        public string UserToId { get; set; }
        public string UserFromId { get; set; }
        public string Searchtext { get; set; }
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }
}