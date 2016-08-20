//using JMM.APEC.BusinessObjects;
using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Calendar.Interfaces
{
    public interface IServiceCalendarDao
    {
        List<Service_CalendarToDo> GetCalendarToDo(ApplicationSystemUser user, string Gateways, string Services, DateTime? Fromdate, DateTime? Todate, int seed, int limit);
    }
}
