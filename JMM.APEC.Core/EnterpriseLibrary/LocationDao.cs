
using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.WebAPI.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class LocationDao : ILocationDao
    {
        private string _databaseName;
        private Db db;

        public LocationDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a System_Country object based on DataReader
        static Func<IDataReader, System_Country> MakeCountry = reader =>
           new System_Country
           {
               Id = reader["countryID"].AsId(),
               Code = reader["countryCode"].AsString(),
               Name = reader["countryName"].AsString()
           };

        // creates a System_County object based on DataReader
        static Func<IDataReader, System_County> MakeCounty = reader =>
           new System_County
           {
               Id = reader["countyId"].AsId(),
               Name = reader["countyName"].AsString(),

               State = new System_State
               {
                   Id = reader["stateId"].AsInt(),
                   Code = reader["stateCode"].AsString(),
                   Name = reader["stateName"].AsString()
               }
           };

        // creates a System_State object based on DataReader
        static Func<IDataReader, System_State> MakeState = reader =>
           new System_State
           {
               Id = reader["stateId"].AsId(),
               Code = reader["stateCode"].AsString(),
               Name = reader["stateName"].AsString(),

               Country = new System_Country
               {
                   Id = reader["countryID"].AsInt(),
                   Code = reader["countryCode"].AsString(),
                   Name = reader["countryName"].AsString()
               }
           };

        // creates a System_TimeZone object based on DataReader
        static Func<IDataReader, System_TimeZone> MakeTimeZone = reader =>
           new System_TimeZone
           {
               Id = reader["id"].AsId(),
               Code = reader["code"].AsString(),
               Name = reader["name"].AsString(),
               GMT = reader["gmt"].AsString(),
               Offset = reader["Offset"].AsInt()
           };

        public List<System_Country> GetCountry(string CountryCode)
        {
            if (CountryCode == "null" || CountryCode == "")
            {
                CountryCode = null;
            }

            string storedProcName = "System_getCountry";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@countryCode", DbType = DbType.String, Value = CountryCode }
            };

            IEnumerable<System_Country> result = db.Read(storedProcName, MakeCountry, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for CountryCode '{0}'.", CountryCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        public List<System_County> GetCounty(int? CountyId, string StateCode)
        {
            if (CountyId == 0 || CountyId == null)
            {
                CountyId = null;
            }

            if (StateCode == "null" || StateCode == "")
            {
                StateCode = null;
            }

            string storedProcName = "System_getCounty";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@countyId", DbType = DbType.Int32, Value = CountyId.HasValue ? CountyId : null },
                new SqlParameter(){ ParameterName="@stateCode", DbType = DbType.String, Value = StateCode }
            };

            IEnumerable<System_County> result = db.Read(storedProcName, MakeCounty, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for CountyId '{0}', StateCode '{1}'.", CountyId, StateCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        public List<System_State> GetState(string CountryCode, string StateCode)
        {

            if (CountryCode == "null" || CountryCode == "")
            {
                CountryCode = null;
            }

            if (StateCode == "null" || StateCode == "")
            {
                StateCode = null;
            }

            string storedProcName = "System_getState";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@countryCode", DbType = DbType.String, Value = CountryCode },
                new SqlParameter(){ ParameterName="@stateCode", DbType = DbType.String, Value = StateCode }
            };

            IEnumerable<System_State> result = db.Read(storedProcName, MakeState, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for CountryCode '{0}', StateCode '{1}'.", CountryCode, StateCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        public List<System_TimeZone> GetTimeZone(int? TimeZoneId, string TimeZoneCode)
        {
            if (TimeZoneCode == "null" || TimeZoneCode == "")
            {
                TimeZoneCode = null;
            }

            string storedProcName = "System_getTimeZone";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inTimeZoneID", DbType = DbType.Int32, Value = TimeZoneId.HasValue ? TimeZoneId : null },
                new SqlParameter(){ ParameterName="@inTimeZoneCode", DbType = DbType.String, Value = TimeZoneCode }
            };

            IEnumerable<System_TimeZone> result = db.Read(storedProcName, MakeTimeZone, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for TimeZoneId '{0}', TimeZoneCode '{1}'.", TimeZoneId, TimeZoneCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

    }
}
