
using JMM.APEC.Core;
using JMM.APEC.IMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS.Interfaces
{
    public interface IServiceFuelManagementDao
    {
        List<Service_TankCompartmentLevelGroup> GetDashboardTankLevels(ApplicationSystemUser user, string Gateways, string Facilities, string Products, int? PercentLevel, int Seed, int Limit);
        List<Service_EventCompartmentLevel> GetFacilityCompartmentTankLevels_Latest(ApplicationSystemUser user, int? FacilityId);
        List<Service_EventCompartmentLevel> GetCompartmentTankLevels(ApplicationSystemUser user, string Gateways, string Facilities, string Products, int? TankCompartmentId, int? Interval, int Seed, int Limit);
        List<Service_EventCompartmentDelivery> GetCompartmentTankDeliveries_Latest(ApplicationSystemUser user, int? TankCompartmentId);
        List<Service_TankCompartmentEvent> GetCompartmentTankEvents(ApplicationSystemUser user, int? TankCompartmentId, string EventTypes, int Seed, int Limit);
    }
}
