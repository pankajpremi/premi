using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.WebAPI.Logging;
using Microsoft.SqlServer.Server;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.DAL;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class SystemTypeDao : ISystemTypeDao
    {

        private string _databaseName;
        private Db db;

        public SystemTypeDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }


        // creates a System_Type object based on DataReader
        static Func<IDataReader, System_Type> Make = reader =>
           new System_Type
           {
              
               GatewayId = reader["gatewayID"].AsInt(),
               ObjectId = reader["ObjectID"].AsInt(),
               TypeCode = reader["code"].AsString(),
               TypeName = reader["Name"].AsString(),
               TypeId = reader["Id"].AsInt()

           };


        DbParameter[] Take(System_Type type)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inId", DbType = DbType.Int32, Value = type.TypeId },
                new SqlParameter(){ ParameterName="@inCode", DbType = DbType.String, Value = type.TypeCode },
                new SqlParameter(){ ParameterName="@inName", DbType = DbType.String, Value = type.TypeName },
                new SqlParameter(){ ParameterName="@inActive", DbType = DbType.Boolean, Value = type.Active },
                new SqlParameter(){ ParameterName="@inSortOrder", DbType = DbType.Int32, Value = type.SortOrder },
                new SqlParameter(){ ParameterName="@inObjectId", DbType = DbType.Int32, Value = type.ObjectId},
                new SqlParameter(){ ParameterName="@inGatewayId", DbType = DbType.Int32, Value = type.GatewayId }
            };

            return parameters;
        }


        public List<System_Type> GetType(ApplicationSystemUser user, int? ObjectId,string ObjectCode,  string GatewayIDs, string TypeCode)
        {
            if( ObjectCode == null || ObjectCode == "")
            {
                ObjectCode = null;
            }

            if(TypeCode == null || TypeCode == "")
            {
                TypeCode = null;
            }

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
                new SqlParameter(){ ParameterName="@inObjectID", DbType = DbType.Int32, Value = ObjectId.HasValue ? ObjectId: null },
                new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = ObjectCode },
                new SqlParameter(){ ParameterName="@inGatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },   
                new SqlParameter(){ ParameterName="@inTypeCode", DbType = DbType.String, Value = TypeCode }
            };

            IEnumerable<System_Type> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for ObjectId '{0}', GatewayIds '{1}', TypeCode '{2}'.", ObjectId, GatewayIDs, TypeCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();


        }
        

        //public void InsertType(System_Type type)
        //{
        //    string storedProcName = "System_insertType";

        //    type.TypeId = db.Insert(storedProcName, Take(type));

        //    if (type.TypeId <= 0)
        //    {
        //        LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
        //    }
        //    else
        //    {
        //        LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + type.TypeId.ToString());
        //    }

        //    return;
        //}

        //public void UpdateType(System_Type type)
        //{
        //    string storedProcName = "System_updateType";

        //    db.Update(storedProcName, Take(type));
        //    LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");

        //    return;
        //}


        //public void DeleteType(System_Type type)
        //{
        //    string storedProcName = "System_deleteCategory";

        //    db.Delete(storedProcName, Take(type));
        //    LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

        //    return;
        //}






    }
}
