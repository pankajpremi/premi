//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActivityLog.Interfaces
{
    public interface IServiceActivityLogDao
    {
        List<Service_ActivityLog> GetAllActivityLogs(ApplicationSystemUser user, string Gateways, string facilities, string ALTypes, DateTime? fromdate, DateTime? todate);
        List<Service_ActivityLogComment> GetActivityLogComments(int ActivityLogId);
        List<Service_ActivityLogMedia> GetActivityLogMedia(int ActivityLogId);
        List<Service_ActivityLog> GetActivityLogsByFacility(int FacilityId);
        List<System_Type> GetActivityLogTypes(ApplicationSystemUser user, int ObjectId, string GatewayIDs, string TypeCode);


    }
}
