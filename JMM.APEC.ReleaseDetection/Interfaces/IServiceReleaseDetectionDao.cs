using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ReleaseDetection.Interfaces
{
    public interface IServiceReleaseDetectionDao
    {
        List<Service_ReleaseDetection> GetReleaseDetectionResults(ApplicationSystemUser user, string Gateways, string Facilities,
            string Results, string RDStatuses, string AssetTypes, DateTime? Fromdate, DateTime? Todate, int? ReleaseDetectionId, int Seed, int Limit);
    }
}
