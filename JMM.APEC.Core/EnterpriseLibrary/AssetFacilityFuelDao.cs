using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.WebAPI.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetFacilityFuelDao : IAssetFacilityFuelDao
    {

        private string _databaseName;
        private Db db;

        public AssetFacilityFuelDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        DbParameter[] Take(Asset_FacilityFuel facilityFuel)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inFacilityFuelID", DbType = DbType.Int32, Value = facilityFuel.FacilityFuelId },
                new SqlParameter(){ ParameterName="@facilityID", DbType = DbType.Int32, Value = facilityFuel.FacilityId },
                new SqlParameter(){ ParameterName="@businessUnit", DbType = DbType.String, Value = facilityFuel.BusinessUnit},
                new SqlParameter(){ ParameterName="@district", DbType = DbType.String, Value = facilityFuel.District },
                new SqlParameter(){ ParameterName="@hasCarWash", DbType = DbType.Boolean, Value = facilityFuel.HasCarWash },
                new SqlParameter(){ ParameterName="@gasBrand", DbType = DbType.String, Value = facilityFuel.GasBrand},
                new SqlParameter(){ ParameterName="@market", DbType = DbType.String, Value = facilityFuel.Market},
                new SqlParameter(){ ParameterName="@operatingHours", DbType = DbType.String, Value = facilityFuel.OperatingHours},
                new SqlParameter(){ ParameterName="@classOfTrade", DbType = DbType.String, Value = facilityFuel.ClassOfTrade },
                new SqlParameter(){ ParameterName="@effectiveOpsDate", DbType = DbType.DateTime, Value = facilityFuel.EffectiveOpsDate== DateTime.MinValue?  (Object)DBNull.Value :facilityFuel.EffectiveOpsDate },
                new SqlParameter(){ ParameterName="@complianceMgmtDate", DbType = DbType.DateTime, Value = facilityFuel.ComplianceMgmtDate== DateTime.MinValue?  (Object)DBNull.Value :facilityFuel.ComplianceMgmtDate },
                new SqlParameter(){ ParameterName="@anticipatedOpsDate", DbType = DbType.DateTime, Value = facilityFuel.AnticipatedOpsDate== DateTime.MinValue?  (Object)DBNull.Value :facilityFuel.AnticipatedOpsDate },
                new SqlParameter(){ ParameterName="@closedDate", DbType = DbType.DateTime, Value = facilityFuel.ClosedDate== DateTime.MinValue?  (Object)DBNull.Value :facilityFuel.ClosedDate },
                new SqlParameter(){ ParameterName="@AppChangeUserId", DbType = DbType.Int32, Value = facilityFuel.AppChangeUserID }
            };

            return parameters;
        }


        public int InsertFacilityFuel(Asset_FacilityFuel FacFuel)
        {
            string storedProcName = "Asset_saveFacilityFuel";

             int FacilityFuelId = db.Insert(storedProcName, Take(FacFuel));

            if (FacilityFuelId <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + FacilityFuelId.ToString());
            }

            return FacilityFuelId;
           
        }


        public int UpdateFacilityFuel(Asset_FacilityFuel FacFuel)
        {
            string storedProcName = "Asset_saveFacilityFuel";

             int FacilityFuelId = db.Update(storedProcName, Take(FacFuel));

            if (FacilityFuelId <= 0)
            {
                LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Failed to insert record");
            }
            else
            {
                LogWriter.Log.DatabaseOutputParams(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), "Record inserted successfully Id = " + FacilityFuelId.ToString());
            }

            return FacilityFuelId;

        }


        public List<Asset_FacilityFuel> GetFacilityFuel(int FacilityId)
        {
            string storedProcName = "Asset_getFacilityFuel";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inFacilityID", DbType = DbType.Int32, Value = FacilityId }
            };

            IEnumerable<Asset_FacilityFuel> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for FacilityId '{0}'.", FacilityId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();

        }

        static Func<IDataReader, Asset_FacilityFuel> Make = reader =>
          new Asset_FacilityFuel
          {
              FacilityFuelId = reader["id"].AsId(),
              FacilityId = reader["FacilityId"].AsInt(),
              BusinessUnit = reader["BusinessUnit"].AsString(),
              District = reader["District"].AsString(),
              HasCarWash = reader["HasCarWash"].AsBool(),
              GasBrand = reader["GasBrand"].AsString(),           
              Market = reader["Market"].AsString(),
              OperatingHours = reader["OperatingHours"].AsString(),
              ClassOfTrade = reader["ClassOfTrade"].AsString(),
              EffectiveOpsDate = reader["EffectiveOpsDate"].AsDateTime(),
              ComplianceMgmtDate = reader["ComplianceMgmtDate"].AsDateTime(),
              AnticipatedOpsDate = reader["AnticipatedOpsDate"].AsDateTime(),
              ClosedDate = reader["ClosedDate"].AsDateTime(),
              AppChangeUserID = reader["AppChangeUserID"].AsInt()
          };

    }
}
