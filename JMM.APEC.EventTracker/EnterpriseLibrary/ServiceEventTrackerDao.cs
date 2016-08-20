//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.EventTracker.Interfaces;
using JMM.APEC.WebAPI.Logging;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.Core;

namespace JMM.APEC.EventTracker.EnterpriseLibrary
{
    public class ServiceEventTrackerDao : IServiceEventTrackerDao
    {
        private string _databaseName;
        private Db db;

        public ServiceEventTrackerDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

         public List<Service_EventTrackerReminder> GetEventTrackerRemindersByFacility(int FacilityId)
        {
            return null;
        }

        public List<Service_EventTrackerReminderList> GetAllEventTrackerReminders(ApplicationSystemUser user,string Gateways, string Facilities, string Statuses, string Categorys, string Types, string SubTypes, DateTime? Fromdate, DateTime? Todate, int seed, int limit)
        {
            string storedProcName = "Service_getEventTrackerReminder";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
            var intStatusList = DbHelpers.MakeParamIntList(Statuses);
            var intCatList = DbHelpers.MakeParamIntList(Categorys);
            var intTypeList = DbHelpers.MakeParamIntList(Types);
            var intSubTypeList = DbHelpers.MakeParamIntList(SubTypes);
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

            List<SqlDataRecord> CatList = DbHelpers.MakeParamRecordList(intCatList, "Id");

            List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

            List<SqlDataRecord> SubTypeList = DbHelpers.MakeParamRecordList(intSubTypeList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@gatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@facilityIDs",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@StatusIDs", SqlDbType = SqlDbType.Structured, Value = StatusList },
                new SqlParameter(){ ParameterName="@categoryIDs", SqlDbType = SqlDbType.Structured, Value = CatList },
                new SqlParameter(){ ParameterName="@typeIDs",  SqlDbType = SqlDbType.Structured, Value = TypeList },
                new SqlParameter(){ ParameterName="@subtypeIDs", SqlDbType = SqlDbType.Structured, Value = SubTypeList },
                new SqlParameter(){ ParameterName="@FromDate", DbType = DbType.DateTime, Value = Fromdate.HasValue ? Fromdate : null },
                new SqlParameter(){ ParameterName="@ToDate", DbType = DbType.DateTime, Value = Todate.HasValue ? Todate : null }
                //new SqlParameter(){ ParameterName="@inSeed", DbType = DbType.Int32, Value = seed },
                //new SqlParameter(){ ParameterName="@inLimit", DbType = DbType.Int32, Value = limit}
            };

            IEnumerable<Service_EventTrackerReminderList> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " User '{0}', Gateways '{1}', Facilities '{2}', Statuses '{3}' Categories '{4}', Types '{5}', SubTypes '{6}',DateFrom '{7}', DateTo '{8}'.", user.UserName, Gateways, Facilities, Statuses, Categorys, Types, SubTypes, Fromdate, Todate);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            //add logic to filter data before sending results



            return result.ToList();
        }



        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Service_EventTrackerReminderList> Make = reader =>
           new Service_EventTrackerReminderList
           {
               EventTrackerReminderId = reader["EventTrackerReminderId"].AsId(),
               GatewayId = reader["gatewayID"].AsInt(),
               ObjectId = reader["objectID"].AsInt(),
               CategoryId = reader["CategoryID"].AsInt(),
               TypeId = reader["TypeID"].AsInt(),
               SubTypeId = reader["SubTypeID"].AsInt(),
               DueDate = reader["DueDate"].AsDateTime(),
               DateCompleted = reader["CompleteDate"].AsDateTime(),
               FacilityId = reader["FacilityId"].AsInt(),
               FacilityName = reader["objectName"].AsString(),
               ParentId = reader["ParentID"].AsInt(),

               CategoryName = reader["categoryName"].AsString(),
               TypeName = reader["typeName"].AsString(),
               SubTypeName = reader["subyTypeName"].AsString(),
               GatewayName = reader["gatewayname"].AsString(),

               Status = reader["status"].AsString(),

           };

        static Func<IDataReader, System_Category> MakeCategory = reader =>
           new System_Category
           {
               
               CategoryId = reader["ID"].AsId(),
               CategoryCode = reader["categoryCode"].AsString(),
               Object = new System_Object
               {

                   Id = reader["ObjectID"].AsInt(),
                   Code = reader["objectCode"].AsString(),
                   Name = reader["objectName"].AsString(),
               },
               Active = reader["isActive"].AsBool(),
               CategoryName = reader["categoryName"].AsString(),
              
           };


        static Func<IDataReader, System_CategoryType> MakeType = reader =>
         new System_CategoryType
         {

             CategoryId = reader["categoryID"].AsInt(),
             TypeId = reader["typeID"].AsId(),
             TypeCode = reader["typeCode"].AsString(),
             TypeName = reader["typeName"].AsString()
                      

         };

        static Func<IDataReader, System_SubType> MakeSubType = reader =>
       new System_SubType
       {

           TypeId = reader["typeID"].AsInt(),
           SubTypeId = reader["subTypeID"].AsId(),
           SubTypeCode = reader["subTypeCode"].AsString(),
           SubTypeName = reader["subTypeName"].AsString()


       };



        public List<System_Category> GetEventCategories(ApplicationSystemUser user,string GatewayIDs, int ObjectID)
        {
           string storedProcName = "System_getCategory";


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
                new SqlParameter(){ ParameterName="@inGatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@inObjectID", DbType = DbType.Int32, Value = ObjectID }
            };

            IEnumerable<System_Category> result = db.Read(storedProcName, MakeCategory, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for GatewayID '{0}', ObjectID {1}'.", GatewayIDs, ObjectID);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

            
        }

        public List<System_CategoryType> GetEventTypes(int ObjectId, string Categorys)
        {
            string storedProcName = "System_getCategoryType";

            var intCatList = DbHelpers.MakeParamIntList(Categorys);
            List<SqlDataRecord> CatList = DbHelpers.MakeParamRecordList(intCatList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inObjectID", DbType = DbType.Int32, Value = ObjectId },
                new SqlParameter(){ ParameterName="@inCategoryIDs", SqlDbType = SqlDbType.Structured, Value = CatList }
            };

            IEnumerable<System_CategoryType> result = db.Read(storedProcName, MakeType, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for ObjectId '{0}', Categorys {1}'.", ObjectId, Categorys);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

       
        public List<System_SubType> GetEventSubTypes(ApplicationSystemUser user,string GatewayIDs, int ObjectId, string Types)
        {
            string storedProcName = "System_getSubType";

            var intTypeList = DbHelpers.MakeParamIntList(Types);
            List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

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
                new SqlParameter(){ ParameterName="@inGatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },            
                new SqlParameter(){ ParameterName="@inObjectID", DbType = DbType.Int32, Value = ObjectId },
                new SqlParameter(){ ParameterName="@inTypeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList }
            };

            IEnumerable<System_SubType> result = db.Read(storedProcName, MakeSubType, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for GatewayId '{0}', ObjectId '{1}', Types {2}'.", GatewayIDs,ObjectId, Types);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

    }
}
