using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.EventTracker.Interfaces
{
    public interface IServiceEventTrackerDao
    {
        List<Service_EventTrackerReminder> GetEventTrackerRemindersByFacility(int FacilityId);
        List<Service_EventTrackerReminderList> GetAllEventTrackerReminders(ApplicationSystemUser user,string Gateways, string Facilities, string Statuses, string Categorys, string Types, string SubTypes, DateTime? Fromdate, DateTime? Todate, int seed, int limit);
        List<System_Category> GetEventCategories(ApplicationSystemUser user,string GatewayIDs, int ObjectID);
        List<System_CategoryType> GetEventTypes(int ObjectId, string Categorys);
        List<System_SubType> GetEventSubTypes(ApplicationSystemUser user, string GatewayIDs, int ObjectId, string Types);
    }
}
