using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.WebAPI.Logging;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.DAL;
using Microsoft.SqlServer.Server;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class SystemObjectDao : ISystemObjectDao
    {
        private string _databaseName;
        private Db db;

        public SystemObjectDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a System_Object object based on DataReader
        //static Func<IDataReader, Asset_PhoneAssignment> MakePhone = reader =>
        //   new Asset_ObjectPhone
        //   {
        //       PhoneId = reader["phoneID"].AsInt(),
        //       Number = reader["objectCode"].AsString(),
        //       TypeId = reader["typeID"].AsInt(),
        //       ObjectId = reader["objectID"].AsInt(),
        //       EntityId = reader["entityID"].AsInt()
        //   };

        //static Func<IDataReader, Asset_ObjectContact> MakeContact = reader =>
        //  new Asset_ObjectContact
        //  {
        //      ContactId = reader["phoneID"].AsInt(),
        //      Title = reader["title"].AsString(),
        //      FirstName = reader["firstName"].AsString(),
        //      LastName = reader["lastName"].AsString(),
        //      AddressId = reader["addressID"].AsInt(),
        //      Email = reader["email"].AsString(),
        //      Active = reader["isActive"].AsBool(),
        //      TypeId = reader["typeID"].AsInt(),
        //      ObjectId = reader["objectID"].AsInt(),
        //      EntityId = reader["entityID"].AsInt()
        //  };

        //static Func<IDataReader, System_ObjectMessage> MakeMessage = reader =>
        //  new System_ObjectMessage
        //  {
        //      MessageId = reader["messageID"].AsInt(),
        //      UserToId = reader["userToID"].AsInt(),
        //      UserFromId = reader["userFromID"].AsInt(),
        //      BeginDateTime = reader["BeginDateTime"].AsDateTime(),
        //      EndDateTime = reader["EndDateTime"].AsDateTime(),
        //      Subject = reader["Subject"].AsString(),
        //      Message = reader["Message"].AsString(),
        //      IsDismissible = reader["isDissmissible"].AsBool(),
        //      ObjectId = reader["objectID"].AsInt(),
        //      EntityId = reader["entityID"].AsInt()
        //  };


        DbParameter[] Take(System_Object obj)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inId", DbType = DbType.Int32, Value = obj.Id },
                new SqlParameter(){ ParameterName="@inCode", DbType = DbType.String, Value = obj.Code },
                new SqlParameter(){ ParameterName="@inName", DbType = DbType.String, Value = obj.Name },
                new SqlParameter(){ ParameterName="@inCategoryId", DbType = DbType.Int32, Value = obj.CategoryId}
            };

            return parameters;
        }


   


        //public List<Asset_ObjectContact> GetContactList(string ObjectCode, string objectId, string EntityId, string TypeId)
        // {
        //     string storedProcName = "Asset_getContactAssignment";

        //     var intObjectList = DbHelpers.MakeParamIntList(objectId);
        //     var intEntityList = DbHelpers.MakeParamIntList(EntityId);
        //     var intTypeList = DbHelpers.MakeParamIntList(TypeId);


        //     List<SqlDataRecord> ObjectList = DbHelpers.MakeParamRecordList(intObjectList, "Id");

        //     List<SqlDataRecord> EntityList = DbHelpers.MakeParamRecordList(intEntityList, "Id");

        //     List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

        //     var parameters = new[]{
        //         new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = ObjectCode },
        //         new SqlParameter(){ ParameterName="@inEntityIDs",SqlDbType = SqlDbType.Structured, Value = ObjectList },
        //         new SqlParameter(){ ParameterName="@inPhoneIDs", SqlDbType = SqlDbType.Structured, Value = EntityList },
        //         new SqlParameter(){ ParameterName="@inTypeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList}

        //     };


        //     //IEnumerable<Asset_ObjectContact> result = db.Read(storedProcName, MakeContact, parameters);

        //     if (result == null || result.Count() == 0)
        //     {
        //         string errorMessage = string.Format("Error - record not found " + " for ObjectCode '{0}'.", ObjectCode);
        //         LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

        //         return null;
        //     }

        //     LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

        //     return result.ToList();
        // }


        //public List<System_ObjectMessage> GetMessageList(string ObjectCode, string objectId, string EntityId, string TypeId)
        //{
        //    string storedProcName = "System_getMessageAssignment";

        //    var intObjectList = DbHelpers.MakeParamIntList(objectId);
        //    var intEntityList = DbHelpers.MakeParamIntList(EntityId);
        //    var intTypeList = DbHelpers.MakeParamIntList(TypeId);


        //    List<SqlDataRecord> ObjectList = DbHelpers.MakeParamRecordList(intObjectList, "Id");

        //    List<SqlDataRecord> EntityList = DbHelpers.MakeParamRecordList(intEntityList, "Id");

        //    List<SqlDataRecord> TypeList = DbHelpers.MakeParamRecordList(intTypeList, "Id");

        //    var parameters = new[]{
        //        new SqlParameter(){ ParameterName="@inObjectCode", DbType = DbType.String, Value = ObjectCode },
        //        new SqlParameter(){ ParameterName="@inEntityIDs",SqlDbType = SqlDbType.Structured, Value = ObjectList },
        //        new SqlParameter(){ ParameterName="@inPhoneIDs", SqlDbType = SqlDbType.Structured, Value = EntityList },
        //        new SqlParameter(){ ParameterName="@inTypeIDs", SqlDbType = SqlDbType.Structured, Value = TypeList}

        //    };


        //   // IEnumerable<System_ObjectMessage> result = db.Read(storedProcName, MakeMessage, parameters);

        //    if (result == null || result.Count() == 0)
        //    {
        //        string errorMessage = string.Format("Error - record not found " + " for ObjectCode '{0}'.", ObjectCode);
        //        LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

        //        return null;
        //    }

        //    LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

        //    return result.ToList();
        //}





        public void InsertObject(System_Object obj)
        {
            string storedProcName = "System_insertObject";

            obj.Id = db.Insert(storedProcName, Take(obj));

            if (obj.Id <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + obj.Id.ToString());
            }

            return;
        }

        public void UpdateObject(System_Object obj)
        {
            string storedProcName = "System_updateObject";

            db.Update(storedProcName, Take(obj));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");

            return;
        }


        public void DeleteObject(System_Object obj)
        {
            string storedProcName = "System_deleteObject";

            db.Delete(storedProcName, Take(obj));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return;
        }



    }
}
