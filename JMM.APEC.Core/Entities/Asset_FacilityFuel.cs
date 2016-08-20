using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_FacilityFuel
    {
        public int FacilityFuelId { get; set; }
        public int FacilityId { get; set; }
        public string District { get; set; }
        public string BusinessUnit { get; set; }
        public bool? HasCarWash{get; set;}
        public string GasBrand { get; set; }
        public string Market { get; set; }
        public string OperatingHours { get; set; }
        public string ClassOfTrade { get; set; }
        public DateTime? EffectiveOpsDate { get; set; }
        public DateTime? ComplianceMgmtDate { get; set; }
        public DateTime? AnticipatedOpsDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int AppChangeUserID { get; set; }
    }
}
