using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL.EnterpriseLibrary;
using System.Data.SqlClient;
using JMM.APEC.WebAPI.Logging;
using System.Reflection;
using System.Data;
using JMM.APEC.DAL;
using Microsoft.SqlServer.Server;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetPhoneDao : IAssetPhoneDao
    {

        private string _databaseName;
        private Db db;

        public AssetPhoneDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Asset_Phone> Make = reader =>
           new Asset_Phone
           {
               PhoneId = reader["id"].AsId(),
               GatewayId = reader["gatewayID"].AsInt(),
               Number = reader["Number"].AsString(),
               AppChangeUserId = reader["AppchangeuserId"].AsInt(),
               TypeId = reader["typeID"].AsInt(),

               Type = new System_Type
               {
                   TypeId = reader["typeID"].AsInt(),
                  TypeName = reader["typeName"].AsString()
               },
               
           };



        public List<Asset_Phone> GetPhone(int GatewayId, string PhoneId)
        {
            
            string storedProcName = "Asset_getPhone";
            var intPhoneList = DbHelpers.MakeParamIntList(PhoneId);
           

            List<SqlDataRecord> PhoneList = DbHelpers.MakeParamRecordList(intPhoneList, "Id");

            var parameters = new[]{
                 new SqlParameter(){ ParameterName="@inGatewayID",DbType = DbType.Int32, Value = GatewayId },
                 new SqlParameter(){ ParameterName="@inPhoneIDs", SqlDbType = SqlDbType.Structured, Value = PhoneList }
            };

            IEnumerable<Asset_Phone> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for PhoneId '{0}'.", PhoneId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();



        }



        System.Data.Common.DbParameter[] Take(Asset_Phone Phone)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inPhoneID", DbType = DbType.Int32, Value = Phone.PhoneId },
                 new SqlParameter(){ ParameterName="@gatewayID", DbType = DbType.Int32, Value = Phone.GatewayId },
                new SqlParameter(){ ParameterName="@number", DbType = DbType.String, Value = Phone.Number },
                new SqlParameter(){ ParameterName="@typeID", DbType = DbType.Int32, Value = Phone.TypeId },
                new SqlParameter(){ ParameterName="@isDeleted", DbType = DbType.Boolean, Value = Phone.IsDeleted},
                new SqlParameter(){ ParameterName="@appChangeUserID", DbType = DbType.Int32, Value = Phone.AppChangeUserId }
               
            };

            return parameters;
        }

        public int InsertPhone(Asset_Phone Phone)
        {
            string storedProcName = "Asset_savePhone";


            Phone.PhoneId = db.Insert(storedProcName, Take(Phone));

            if (Phone.PhoneId <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + Phone.PhoneId.ToString());
            }

            return Phone.PhoneId;
        }


        public int UpdatePhone(Asset_Phone Phone)
        {
            string storedProcName = "Asset_savePhone";


            int PhoneId = db.Update(storedProcName, Take(Phone));
            LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");

            return PhoneId;
        }

        public void DeletePhone(Asset_Phone Phone)
        {
            string storedProcName = "Asset_deletePhone";

            db.Delete(storedProcName, Take(Phone));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return;
        }



       public List<Asset_PhoneAssignment> GetPhoneAssignment(string ObjectCode, string EntityId, string PhoneId, string TypeId)
        {

            string storedProcName = "Asset_getPhoneAssignment";

            var intEntityList = DbHelpers.MakeParamIntList(EntityId);
            var intPhoneList = DbHelpers.MakeParamIntList(PhoneId);
            var intTypeList = DbHelpers.MakeParamIntList(TypeId);
         

            List<SqlDataRecord> EntityList = DbHelpers.MakeParamRecordList(intEntityList, "Id");

            List<SqlDataRecord> PhoneList = DbHelpers.MakeParamRecordList(intPhoneList, "Id");

            List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = ObjectCode },
                new SqlParameter(){ ParameterName="@inEntityIDs", SqlDbType = SqlDbType.Structured, Value = EntityList },
                new SqlParameter(){ ParameterName="@inPhoneIDs",SqlDbType = SqlDbType.Structured, Value = PhoneList },
                new SqlParameter(){ ParameterName="@inTypeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList }     

            };

            IEnumerable<Asset_PhoneAssignment> result = db.Read(storedProcName, MakePhoneAssignment, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for ObjectCode '{0}' EntityId '{1}' PhoneId '{2}' TypeId '{3}'  .", ObjectCode, EntityId, PhoneId, TypeId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();


        }

        public int SavePhoneAssignment(Asset_PhoneAssignmentEntity obj)
        {
            string storedProcName = "Asset_savePhoneAssignmentByEntity";

            object retval = 0;

            retval = db.Scalar(storedProcName, TakePhoneAssignment(obj));

            if (retval.AsInt() < 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "DB operation failed");
                return -1;
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "DB operation succeeded");
                return 0;
            }

            
        }


        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Asset_PhoneAssignment> MakePhoneAssignment = reader =>
           new Asset_PhoneAssignment
           {
               Id = reader["phoneAssignmentID"].AsId(),
               Phone = new Asset_Phone
               {
                   PhoneId = reader["phoneID"].AsInt(),
                   Number = reader["Number"].AsString(),
                   TypeId = reader["typeid"].AsInt(),
               },

               ObjectId = reader["objectid"].AsInt(),
               EntityId = reader["entityID"].AsInt()
           };


        System.Data.Common.DbParameter[] TakePhoneAssignment(Asset_PhoneAssignmentEntity obj)
        {
            var intPhoneList = DbHelpers.MakeParamIntList(obj.PhoneId);

            List<SqlDataRecord> PhoneList = DbHelpers.MakeParamRecordList(intPhoneList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = obj.ObjectCode },
                new SqlParameter(){ ParameterName="@inEntityID", DbType = DbType.Int32, Value = obj.EntityId },
                new SqlParameter(){ ParameterName="@inPhoneIDs", SqlDbType = SqlDbType.Structured, Value = PhoneList}

           };

            return parameters;
        }
    }
}
