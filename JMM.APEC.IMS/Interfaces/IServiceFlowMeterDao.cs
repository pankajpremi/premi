//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS.Interfaces
{
    public interface IServiceFlowMeterDao
    {
        List<Service_FlowMeterGroup> GetDashboardFlowMeters(ApplicationSystemUser user, string Gateways, string Facilities, decimal? GMPLevel, int Seed, int Limit);
        List<Service_FlowMeter> GetFacilityFlowMeters_Latest(ApplicationSystemUser user, int? FacilityId);
        List<Service_FlowMeter> GetFuelPositionFlowMeters(ApplicationSystemUser user, string Gateways, string Facilities, int? FuelPositionId, int? Interval, int Seed, int Limit);
        List<Service_FlowMeterEvent> GetFlowMeterEvents(ApplicationSystemUser user, int? FuelPositionId, int Seed, int Limit);
    }
}
