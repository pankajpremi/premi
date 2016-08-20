using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.WebAPI.Logging;
using JMM.APEC.DAL.EnterpriseLibrary;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class SystemCategoryDao : ISystemCategoryDao
    {
        private string _databaseName;
        private Db db;

        public SystemCategoryDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a System_Category object based on DataReader
        static Func<IDataReader, System_Category> Make = reader =>
           new System_Category
           {
               CategoryId = reader["CategoryID"].AsId(),
               GatewayId = reader["gatewayID"].AsInt(),
               ObjectId = reader["ObjectID"].AsInt(),

               Object = new System_Object
               {
                   Code = reader["objectCode"].AsString(),
                   Name = reader["objectName"].AsString(),
               },

               CategoryCode = reader["code"].AsString(),
               CategoryName = reader["Name"].AsString(),
               Active = reader["categoryIsActive"].AsBool()
           };


        DbParameter[] Take(System_Category category)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inId", DbType = DbType.Int32, Value = category.CategoryId },
                new SqlParameter(){ ParameterName="@inCode", DbType = DbType.String, Value = category.CategoryCode },
                new SqlParameter(){ ParameterName="@inName", DbType = DbType.String, Value = category.CategoryName },
                new SqlParameter(){ ParameterName="@inActive", DbType = DbType.Boolean, Value = category.Active },
                new SqlParameter(){ ParameterName="@inSortOrder", DbType = DbType.Int32, Value = category.SortOrder },
                new SqlParameter(){ ParameterName="@inObjectId", DbType = DbType.Int32, Value = category.ObjectId},
                new SqlParameter(){ ParameterName="@inGatewayId", DbType = DbType.Int32, Value = category.GatewayId }
            };

            return parameters;
        }

        public List<System_Category> GetCategories(int? GatewayId, int? ObjectId, bool? Active)
        {
            string storedProcName = "System_getCategories";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inGatewayId", DbType = DbType.Int32, Value = GatewayId.HasValue ? GatewayId: null  },
                new SqlParameter(){ ParameterName="@inObjectId", DbType = DbType.String, Value = ObjectId.HasValue ? ObjectId: null },
                new SqlParameter(){ ParameterName="@inActive", DbType = DbType.Boolean, Value = Active.HasValue ? Active: null }
            };

            IEnumerable<System_Category> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for GatewayId '{0}', ObjectId '{1}', Active '{2}'.", GatewayId, ObjectId, Active);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        public void InsertCategory(System_Category category)
        {
            string storedProcName = "System_insertCategory";

            category.CategoryId = db.Insert(storedProcName, Take(category));

            if (category.CategoryId <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + category.CategoryId.ToString());
            }

            return;
        }

        public void UpdateCategory(System_Category category)
        {
            string storedProcName = "System_updateCategory";

            db.Update(storedProcName, Take(category));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");

            return;
        }


        public void DeleteCategory(System_Category category)
        {
            string storedProcName = "System_deleteCategory";

            db.Delete(storedProcName, Take(category));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return;
        }

    }
}
