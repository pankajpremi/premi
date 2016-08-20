using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.WebAPI.Logging;
using System.Data.Common;
using System.Data.SqlClient;
using JMM.APEC.DAL.EnterpriseLibrary;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetPortalDao : IAssetPortalDao
    {
        private string _databaseName;
        private Db db;

        public AssetPortalDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
        }

        public List<Asset_Portal> GetPortal(int? PortalId, bool? IsActive)
        {
            db = new Db(_databaseName);

            string storedProcName = "Asset_getPortal";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inPortalID", DbType = DbType.Int32, Value = PortalId.HasValue ? PortalId : null },
                new SqlParameter(){ ParameterName="@inIsActive", DbType = DbType.Boolean, Value = IsActive.HasValue ? IsActive : null }
            };

            IEnumerable<Asset_Portal> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for PortalId '{0}', IsActive '{1}'.", PortalId, IsActive);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }


        public void InsertPortal(Asset_Portal portal)
        {
            string storedProcName = "Asset_insertPortal";

            portal.Id = db.Insert(storedProcName, Take(portal));

            if (portal.Id <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + portal.Id.ToString());
            }

            return;
        }

        public void UpdatePortal(Asset_Portal portal)
        {
            string storedProcName = "Asset_updatePortal";

            db.Update(storedProcName, Take(portal));

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record updated successfully");
            return;
        }

        public void DeletePortal(Asset_Portal portal)
        {
            string storedProcName = "Asset_deletePortal";

            db.Delete(storedProcName, Take(portal));
            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record deleted successfully");

            return;
        }



        DbParameter[] Take(Asset_Portal portal)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inId", DbType = DbType.Int32, Value = portal.Id },
                new SqlParameter(){ ParameterName="@inName", DbType = DbType.String, Value = portal.Name },
                new SqlParameter(){ ParameterName="@inDomainUrl", DbType = DbType.String, Value = portal.DomainUrl },
                new SqlParameter(){ ParameterName="@inActive", DbType = DbType.Boolean, Value = portal.Active },
                new SqlParameter(){ ParameterName="@inAppChangeUserId", DbType = DbType.Int32, Value = portal.ModifiedUserId }
                
            };

            return parameters;
        }


        // creates a Asset_Portal object based on DataReader
        static Func<IDataReader, Asset_Portal> Make = reader =>
           new Asset_Portal
           {
               Id = reader["portalID"].AsId(),
               Name = reader["portalName"].AsString(),
               DomainUrl = reader["domainURL"].AsString(),
               Active = reader["isActive"].AsBool()
           };
    }
}
