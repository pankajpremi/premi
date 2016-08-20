using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Core;

namespace JMM.APEC.Alarm.Interfaces
{
   public interface IServiceAlarmDao
    {
        List<Service_AlarmList> GetCriticalAlarmsForUser(ApplicationSystemUser user);
        List<Service_AlarmEvent> GetCriticalAlarms(string GatewayIds);
        List<Service_AlarmList> GetAlarmsList(ApplicationSystemUser user,string Gateways, string facilities, string statuses, string slas, DateTime? fromdate, DateTime? todate, string search, int? seed, int? limit, string sortcolumn, string sortorder);
        List<Service_AlarmInfo> GetAlarmById(int AlarmEventId);
        List<Service_AlarmEvent> GetPastSlaAlarmsForUser(ApplicationSystemUser user);
        List<Service_AlarmEvent> GetPastSlaAlarmsList(int GatewayId);
        List<Service_AlarmWorklog> GetAlarmWorkLogs(int AlarmEventId);

        List<Service_AlarmDetails> GetAlarmDetailsById(int AlarmEventId);
        List<Asset_Facility> GetFacilityByAlarmId(int AlarmEventId);

        List<Service_AlarmAtgEvents> GetAlarmAtgEvents(int AlarmEventId);
    }
}
