using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetFacilityBindingModel
    {
        public int? FacilityId { get; set; }
        public int? StatusId { get; set; }
        public int? TypeId { get; set; }
        public string GatewayCode { get; set; }
        public int? GatewayStatusId { get; set; }
        public string Searchtext { get; set; }
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }
}