using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetContactBindingModel
    {
        public int GatewayId { get; set; }
        public string ContactId { get; set; }
        public string ObjectCode { get; set; }
        public int? TypeId { get; set; }
        public bool Active { get; set; }
        public string FacilityId { get; set; }
        public string Searchtext { get; set; }
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }


    }
}