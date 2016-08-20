using JMM.APEC.Core;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.Alarm.Interfaces;
using JMM.APEC.WebAPI.Logging;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Alarm.EnterpriseLibrary
{
    public class ServiceAlarmDao : IServiceAlarmDao
    {
        private string _databaseName;
        private Db db;

        public ServiceAlarmDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        public List<Service_AlarmList> GetCriticalAlarmsForUser(ApplicationSystemUser user)
        {
             //for demo purpose only. Need to apply logic to pull Critical alarms for user
            List<Service_AlarmList> AlarmsList = GetAlarmsList(user, null, null, null, null, null, null, null, null, null, null, null);

            if (AlarmsList != null)
            {
                if (AlarmsList.Count > 0)
                {
                    //get top 10 alarms by alarmdate desc
                    var criticalalarms = (from al in AlarmsList orderby al.AlarmEventId descending select al).Take(10);

                    return criticalalarms.ToList();

                }
            }


            return null;

        }

        public List<Service_AlarmEvent> GetCriticalAlarms(string Gateways)
        {
                        return null;

        }

        // creates a System_Category object based on DataReader
        static Func<IDataReader, Service_AlarmEvent> Make = reader =>
           new Service_AlarmEvent
           {
               AlarmEventId = reader["Id"].AsId(),
               AlarmDateTime = reader["AlarmDateTime"].AsDateTime(),
               ReceivedDateTime = reader["ReceivedDateTime"].AsDateTime(),
               RemindDateTime = reader["RemindDateTime"].AsDateTime(),
               DispatchDateTime = reader["DispatchDateTime"].AsDateTime(),
               ClearDateTime = reader["ClearDateTime"].AsDateTime(),

               Alarm = new Service_Alarm
               {
                   AtgTypeId = reader["AlarmTypeId"].AsInt(),
                   Name = reader["Name"].AsString(),
                   Active = reader["Active"].AsBool(),
                   AlarmCategory = new Service_AlarmCategory
                   {
                       AtgCategoryId = reader["AlarmCategoryId"].AsInt(),
                       Name = reader["Name"].AsString(),
                       Active = reader["Active"].AsBool()
                       
                   },

                   SLA = new System_SLA
                   {
                       SLAId = reader["SlaId"].AsInt(),
                       SLAName = reader["Name"].AsString(),
                       Active = reader["Active"].AsBool(),
                       ResponseMinutes = reader["Active"].AsInt()
                   }
               },

               ResolutionCategory = new System_Category
               {
                   CategoryId = reader["ResolutionCategoryId"].AsInt(),
                   CategoryName = reader["Name"].AsString(),
                   Active = reader["Active"].AsBool()
               },

               ResolutionType = new System_Type
               {
                   TypeId = reader["ResolutionTypeId"].AsInt(),
                   TypeName = reader["Name"].AsString(),
                   Active = reader["Active"].AsBool()
               },

               JMMStatus = new System_Status
               {
                   Id = reader["StatusId"].AsInt(),
                   
               },

              PriorityStatus = new System_Status
               {
                   Id = reader["PriorityStatusId"].AsInt(),

               },

              Sensor = new Asset_Sensor
              {
                  SensorId = reader["PriorityStatusId"].AsInt(),
                  Description = reader["Description"].AsString()
              }

           };

        static Func<IDataReader, Service_AlarmList> MakeAlarmList = reader =>
          new Service_AlarmList
          {
              AlarmEventId = reader["alarmeventId"].AsId(),
              AlarmCategory = reader["AlarmCategoryname"].AsString(),
              AlarmType= reader["alarmname"].AsString(),
              GatewayName = reader["gatewayname"].AsString(),
              FacilityName = reader["facilityname"].AsString(),
              FacilityId = reader["FacilityID"].AsInt(),
              gatewayId = reader["GatewayID"].AsInt(),
              JMMStatus = reader["alarmStatusDesc"].AsString(),
              Sensor = reader["Id"].AsString(),
              SensorId = reader["sensorId"].AsInt(),
              ActiveAlarmsCount = reader["active"].AsInt(),
              DurationInSeconds = reader["totalseconds"].AsDouble()
          };


        static Func<IDataReader, Service_AlarmInfo> MakeAlarm = reader =>
          new Service_AlarmInfo
          {
              AlarmEventId = reader["alarmeventId"].AsId(),
              
                  AlarmId = reader["alarmId"].AsId(),
                  AlarmName = reader["alarmname"].AsString(),
                  AlarmCode = reader["alarmCode"].AsString(),
                  SLAId = reader["SlaId"].AsInt(),
                  SLADue = reader["sladue"].AsDateTime(),
              //Active = reader["Active"].AsBool(),
              //ResponseMinutes = reader["Active"].AsInt()          

              Status = reader["Status"].AsString(),
              FacilityName = reader["facilityname"].AsString(),
                  GatewayName = reader["gatewayname"].AsString(),             
                  GatewayId = reader["gatewayId"].AsInt()

          };

        static Func<IDataReader, Service_AlarmDetails> MakeAlarmDetails = reader =>
        new Service_AlarmDetails
        {
            AlarmEventId = reader["alarmeventId"].AsId(),

            AlarmId = reader["alarmId"].AsId(),
            AlarmName = reader["alarmname"].AsString(),
            AlarmCode = reader["alarmCode"].AsString(),
            SLAId = reader["SlaId"].AsInt(),
            AlarmDateTime = reader["AlarmDateTime"].AsDateTime(),
            SensorId = reader["SensorId"].AsInt(),
            Sensor = reader["Sensor"].AsString(),
            Status = reader["Status"].AsString(),
            StatusId = reader["StatusId"].AsInt(),

            SLADue = reader["sladue"].AsDateTime(),
            //Active = reader["Active"].AsBool(),
            //ResponseMinutes = reader["Active"].AsInt()          


            FacilityName = reader["facilityname"].AsString(),
            GatewayName = reader["gatewayname"].AsString(),
            GatewayId = reader["gatewayId"].AsInt()

        };

        public List<Service_AlarmList> GetAlarmsList(ApplicationSystemUser user,string Gateways, string Facilities, string Statuses, string Slas, DateTime? Fromdate, DateTime? Todate, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            string storedProcName = "Service_getAlarmEvents";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
            var intStatusList = DbHelpers.MakeParamIntList(Statuses);
            var intSlaList = DbHelpers.MakeParamIntList(Slas);
            var intUserGatewayList = (from g in user.Gateways select g.PortalGatewayId).ToList();

            List<int> intFinalGatewayList = null;

            if (intGatewayList == null)
            {
                intFinalGatewayList = intUserGatewayList;
            }
            else
            {
                intFinalGatewayList = (from a in intUserGatewayList join b in intGatewayList on a equals b select a).ToList();
            }

            List<SqlDataRecord> GatewayList = DbHelpers.MakeParamRecordList(intFinalGatewayList, "Id");

            List<SqlDataRecord> FacilityList = DbHelpers.MakeParamRecordList(intFacilityList, "Id");

            List<SqlDataRecord> StatusList = DbHelpers.MakeParamRecordList(intStatusList, "Id");

            List<SqlDataRecord> SlaList = DbHelpers.MakeParamRecordList(intSlaList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@gatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@facilityIDs",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@StatusIDs", SqlDbType = SqlDbType.Structured, Value = StatusList },
                new SqlParameter(){ ParameterName="@SlaIDs", SqlDbType = SqlDbType.Structured, Value = SlaList },
                new SqlParameter(){ ParameterName="@Fromdate", DbType = DbType.DateTime, Value = Fromdate },
                new SqlParameter(){ ParameterName="@Todate", DbType = DbType.DateTime, Value = Todate },
                new SqlParameter() {ParameterName="@Search", DbType= DbType.String, Value=Search },
                new SqlParameter(){ ParameterName="@PageNum", DbType = DbType.Int32, Value = Seed },
                new SqlParameter(){ ParameterName="@RecordsPerPage", DbType = DbType.Int32, Value = Limit },
                new SqlParameter() {ParameterName="@SortCol", DbType= DbType.String, Value=Sortcolumn },
                new SqlParameter() {ParameterName="@Sortdir", DbType= DbType.String, Value=Sortorder }
            };

            IEnumerable<Service_AlarmList> result = db.Read(storedProcName, MakeAlarmList, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for Gateways '{0}', Facilities '{1}', statuses '{2}', slas '{3}', fromdate '{4}', Todate '{5}',Search '{6}', Seed '{7}', Limit '{8}',SortColumn '{9}', SortOrder '{10}'.", Gateways, Facilities, Statuses, Slas, Fromdate,Todate, Search, Seed, Limit, Sortcolumn, Sortorder);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();


        }

        public List<Service_AlarmInfo> GetAlarmById(int AlarmEventId)
        {
            string storedProcName = "Service_getAlarmById";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inAlarmEventId", DbType = DbType.Int32, Value = AlarmEventId }
            };

            IEnumerable<Service_AlarmInfo> result = db.Read(storedProcName, MakeAlarm, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for AlarmEventId '{0}'", AlarmEventId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        public List<Service_AlarmDetails> GetAlarmDetailsById(int AlarmEventId)
        {
            string storedProcName = "Service_getAlarmDetailsById";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inAlarmEventId", DbType = DbType.Int32, Value = AlarmEventId }
            };

            IEnumerable<Service_AlarmDetails> result = db.Read(storedProcName, MakeAlarmDetails, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for AlarmEventId '{0}'", AlarmEventId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        public List<Asset_Facility> GetFacilityByAlarmId(int AlarmEventId)
        {

            string storedProcName = "Asset_getFacilityByAlarmId";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inAlarmEventId", DbType = DbType.Int32, Value = AlarmEventId }
            };

            IEnumerable<Asset_Facility> result = db.Read(storedProcName, MakeFacility, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for AlarmEventId '{0}'", AlarmEventId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }


        public List<Service_AlarmAtgEvents> GetAlarmAtgEvents(int AlarmEventId)
        {
            string storedProcName = "Service_getAlarmAtgEvents";

            var parameters = new[]{
               new SqlParameter(){ ParameterName="@inAlarmEventId", DbType = DbType.Int32, Value = AlarmEventId }
            };

            IEnumerable<Service_AlarmAtgEvents> result = db.Read(storedProcName, MakeAlarmAtgEvent, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for AlarmEventId '{0}'", AlarmEventId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }


        public List<Service_AlarmEvent> GetPastSlaAlarmsForUser(ApplicationSystemUser user)
        {
            string storedProcName = "Service_getAlarmEvents_PastSlas";

            var intUserGatewayList = (from g in user.Gateways select g.PortalGatewayId).ToList();

            List<int> intFinalGatewayList = null;

            intFinalGatewayList = intUserGatewayList.ToList();


            List<SqlDataRecord> GatewayList = DbHelpers.MakeParamRecordList(intFinalGatewayList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inGateways", SqlDbType = SqlDbType.Structured, Value = GatewayList }

            };

            IEnumerable<Service_AlarmEvent> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for user '{0}'.", user.UserName);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        public List<Service_AlarmEvent> GetPastSlaAlarmsList(int GatewayId)
        {
            return null;
        }

        public List<Service_AlarmWorklog> GetAlarmWorkLogs(int AlarmEventId)
        {
            string storedProcName = "Service_getComment";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@entityID", DbType = DbType.Int32, Value = AlarmEventId }
            };

            IEnumerable<Service_AlarmWorklog> result = db.Read(storedProcName, MakeWorkLog, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for AlarmEventId '{0}'", AlarmEventId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Service_AlarmWorklog> MakeWorkLog = reader =>
           new Service_AlarmWorklog
           {
               CommentId = reader["commentid"].AsId(),
               AlarmEventId = reader["entityid"].AsInt(),
               Haschild = reader["hasChild"].AsBool(),
               ParentId = reader["parentid"].AsInt(),
               Comment = reader["comment"].AsString(),
               EnteredBy = reader["enteredBy"].AsString(),
               EnteredByUserId = reader["UserId"].AsInt(),
               CommentDateTime = reader["commentDatetime"].AsDateTime(),
               AppChangeUserId = reader["appchangeuserid"].AsInt()              

           };


        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Asset_Facility> MakeFacility = reader =>
           new Asset_Facility
           {
               Id = reader["facilityID"].AsId(),
               GatewayId = reader["gatewayid"].AsInt(),
               Name = reader["facilityName"].AsString(),
               AKAName = reader["akaname"].AsString(),

               Address = new Asset_Address
               {
                   Id = reader["addressID"].AsId(),
                   Address1 = reader["address1"].AsString(),
                   Address2 = reader["address2"].AsString(),
                   City = reader["city"].AsString(),

                   State = new System_State
                   {
                       Id = reader["stateId"].AsId(),
                       Name = reader["state"].AsString(),
                   },

                   PostalCode = reader["postalcode"].AsString()
                   

               }

               
           };


        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Service_AlarmAtgEvents> MakeAlarmAtgEvent = reader =>
           new Service_AlarmAtgEvents
           {
               Type = reader["Type"].AsString(),
               Information = reader["Information"].AsString(),
               Event = reader["Information"].AsString(),
               Time =  reader["Time"].AsDateTime()

           };


    }
}
