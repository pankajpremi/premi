using JMM.APEC.Calendar.Interfaces;
using JMM.APEC.Core;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
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

namespace JMM.APEC.Calendar.EnterpriseLibrary
{
   public class ServiceCalendarDao : IServiceCalendarDao
    {


        private string _databaseName;
        private Db db;

        public ServiceCalendarDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }


        public List<Service_CalendarToDo> GetCalendarToDo(ApplicationSystemUser user, string Gateways, string Services, DateTime? Fromdate, DateTime? Todate, int seed, int limit)
        {
            string storedProcName = "Service_getCalendarToDo";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intServiceList = DbHelpers.MakeParamIntList(Services);
           
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

            List<SqlDataRecord> ServiceList = DbHelpers.MakeParamRecordList(intServiceList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@gatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@serviceIDs",SqlDbType = SqlDbType.Structured, Value = ServiceList },
                new SqlParameter(){ ParameterName="@FromDate", DbType = DbType.DateTime, Value = Fromdate},
                new SqlParameter(){ ParameterName="@ToDate", DbType = DbType.DateTime, Value = Todate}
                //new SqlParameter(){ ParameterName="@inSeed", DbType = DbType.Int32, Value = seed },
                //new SqlParameter(){ ParameterName="@inLimit", DbType = DbType.Int32, Value = limit}
            };

            IEnumerable<Service_CalendarToDo> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " User '{0}', Gateways '{1}', Services '{2}', FromDate '{3}' ToDate '{4}'.", user.UserName, Gateways, Services, Fromdate, Todate);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            //add logic to filter data before sending results



            return result.ToList();
        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Service_CalendarToDo> Make = reader =>
           new Service_CalendarToDo
           {
               EventTrackerReminderId = reader["EventTrackerReminderId"].AsId(),
               FacilityName = reader["facilityName"].AsString(),
               SubjectName = reader["subject"].AsString(),
               ServiceName = reader["servicename"].AsString(),
               GatewayName = reader["gatewayname"].AsString(),
               Status = reader["status"].AsString(),
               Duedate = reader["DueDate"].AsDateTime()
           };

    }
}
