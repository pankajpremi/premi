using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetFacilityDetailsBindingModel
    {
        public string FacilityName { get; set; }
        public string FacilityAKA { get; set; }
        public int? TypeId { get; set; }
        public int? OwnerId { get; set; }
        public int? OwnerStatusId { get; set; }
        public int StatusId { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ComplianceMgmtDate { get; set; }
        public DateTime? EffectiveOpsDate { get; set; }
        public DateTime? AnticipatedOpsDate { get; set; }
        public int? BillingTemplateId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CornerAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        //public int? CountyId { get; set; }
        //public int? StateId { get; set; }
        //public int? CountryId { get; set; }
        //public int? TimeZoneId { get; set; }
        public string CountyCode { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string TimeZoneCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int AppChangeUserId { get; set; }
    }
}