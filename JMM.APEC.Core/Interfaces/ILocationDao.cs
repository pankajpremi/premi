using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface ILocationDao
    {
        List<System_TimeZone> GetTimeZone(int? TimeZoneId, string TimeZoneCode);
        List<System_Country> GetCountry(string CountryCode);
        List<System_County> GetCounty(int? CountyId, string StateCode);
        List<System_State> GetState(string CountryCode, string StateCode);
    }
}
