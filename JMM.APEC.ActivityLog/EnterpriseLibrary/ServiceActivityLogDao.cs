using JMM.APEC.ActivityLog.Interfaces;
using JMM.APEC.Core;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
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

namespace JMM.APEC.ActivityLog.EnterpriseLibrary
{
    public class ServiceActivityLogDao : IServiceActivityLogDao
    {
        private string _databaseName;
        private Db db;

        public ServiceActivityLogDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Service_ActivityLog> Make = reader =>
           new Service_ActivityLog
           {
               ActivityLogId = reader["ActivityLogId"].AsId(),
               Title = reader["Title"].AsString(),
               Body = reader["Body"].AsString(),
               LogDate = reader["LogDateTime"].AsDateTime(),
               UserId = reader["UserID"].AsInt(),
               EnteredBy = reader["EnteredBy"].AsString(),
               EntityId = reader["FacilityId"].AsInt(),
               FacilityName = reader["FacilityName"].AsString(),
               Totalcomments = reader["NumOfComment"].AsInt(),
               IsDeleted = reader["isDeleted"].AsBool(),
               GatewayId = reader["GatewayID"].AsInt(),

               //obj = new System_Object
               //{
               //    Id = reader["ObjectID"].AsInt(),
               //    Name = reader["objectName"].AsString(),
               //    Code = reader["objectCode"].AsString()
               //},

               //Gateway = new Asset_Gateway
               //{
               //    Id = reader["GatewayID"].AsId(),

               //},

               //Type = new System_Type
               //{
               //    TypeId = reader["typeID"].AsId(),
               //    TypeName = reader["TypeName"].AsString()

               //}
           };

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Service_ActivityLogComment> MakeALComment = reader =>
           new Service_ActivityLogComment
           {
               ActivityLogId = reader["ActivityLogId"].AsId(),
               CommentId = reader["commentID"].AsInt(),
               Haschild = reader["hasChild"].AsBool(),
               Comment = reader["comment"].AsString(),
               EnteredByUserId = reader["userID"].AsInt(),
               EnteredBy = reader["enteredBy"].AsString(),
               CommentDateTime = reader["commentDateTime"].AsDateTime(),
               CommentParentId = reader["parentID"].AsInt(),
               AppChangeUserId = reader["appChangeUserID"].AsInt(),
               IsDeleted = reader["isDeleted"].AsBool()


           };


        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Service_ActivityLogMedia> MakeALMedia = reader =>
           new Service_ActivityLogMedia
           {
               ActivityLogId = reader["activityLogID"].AsId(),
               MediaId = reader["mediaID"].AsInt(),
               TypeId = reader["typeID"].AsInt(),
               CategoryId = reader["categoryID"].AsInt(),
               Name = reader["name"].AsString(),
               Description = reader["description"].AsString(),
               FileName = reader["FileName"].AsString(),
               FilePath = reader["FilePath"].AsString(),
               FileSize = reader["fileSize"].AsDouble(),
               IsDeleted = reader["isDeleted"].AsBool(),
               LogDateTime = reader["LogDateTime"].AsDateTime(),
               AppChangeUserId = reader["AppChangeUserId"].AsInt()


           };

        static Func<IDataReader, System_Type> MakeType = reader =>
           new System_Type
           {

               GatewayId = reader["gatewayID"].AsInt(),
               ObjectId = reader["ObjectID"].AsInt(),
               TypeCode = reader["code"].AsString(),
               TypeName = reader["Name"].AsString(),
               TypeId = reader["ID"].AsId()

           };

        public List<Service_ActivityLog> GetActivityLogsByFacility(int FacilityId)
        {
            return null;

        }
        public List<Service_ActivityLog> GetAllActivityLogs(ApplicationSystemUser user, string Gateways, string Facilities, string ALTypes, DateTime? Fromdate, DateTime? Todate)
        {
            string storedProcName = "Service_getActivityLog";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
            var intALTypeList = DbHelpers.MakeParamIntList(ALTypes);
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

            List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intALTypeList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@gatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@facilityIDs",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@TypeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList },
                new SqlParameter(){ ParameterName="@inDateFrom", DbType = DbType.DateTime, Value = Fromdate.HasValue ? Fromdate : null },
                new SqlParameter(){ ParameterName="@inDateTo", DbType = DbType.DateTime, Value = Todate.HasValue ? Todate : null }

            };

            IEnumerable<Service_ActivityLog> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " User '{0}', Gateways '{1}', Facilities '{2}', ALTypes '{3}' ,DateFrom '{4}', DateTo '{5}'.", user.UserName, Gateways, Facilities, ALTypes, Fromdate, Todate);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

        public List<Service_ActivityLogComment> GetActivityLogComments(int ActivityLogId)
        {
            string storedProcName = "Service_getActivityLogComment";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@activityLogID", DbType = DbType.Int32, Value = ActivityLogId }
            };

            IEnumerable<Service_ActivityLogComment> result = db.Read(storedProcName, MakeALComment, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " ActivityLogId '{0}'.", ActivityLogId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

        public List<Service_ActivityLogMedia> GetActivityLogMedia(int ActivityLogId)
        {
            string storedProcName = "Service_getActivityLogMedia";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@activityLogID", DbType = DbType.Int32, Value = ActivityLogId }
            };

            IEnumerable<Service_ActivityLogMedia> result = db.Read(storedProcName, MakeALMedia, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " ActivityLogId '{0}'.", ActivityLogId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

        public List<System_Type> GetActivityLogTypes(ApplicationSystemUser user, int ObjectId, string GatewayIDs, string TypeCode)
        {
            string storedProcName = "System_getType";

            var intGatewayList = DbHelpers.MakeParamIntList(GatewayIDs);

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

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inObjectID", DbType = DbType.Int32, Value = ObjectId },
                new SqlParameter(){ ParameterName="@inGatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@inTypeCode", DbType = DbType.String, Value = TypeCode }
            };

            IEnumerable<System_Type> result = db.Read(storedProcName, MakeType, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for ObjectId '{0}', GatewayIds '{1}', TypeCode '{2}'.", ObjectId, GatewayIDs, TypeCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }



    }
}
