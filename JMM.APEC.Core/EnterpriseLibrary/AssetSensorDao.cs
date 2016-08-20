using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.WebAPI.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class AssetSensorDao : IAssetSensorDao
    {
        private string _databaseName;
        private Db db;

        public AssetSensorDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }



        public List<Asset_SensorAsset> GetSensorAssetDetails(int AlarmEventId)
        {
            string storedProcName = "Asset_getSensorAssignmentByAlarm";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inAlarmEventID", DbType = DbType.Int32, Value = AlarmEventId }
                
            };

            IEnumerable<Asset_SensorAsset> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for AlarmEventId '{0}'.", AlarmEventId);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, Asset_SensorAsset> Make = reader =>
           new Asset_SensorAsset
           {
               SensorId = reader["sensorID"].AsInt(),
               Asset = reader["Asset"].AsString(),
               AtgName = reader["AtgName"].AsString(),
               SensorLabel = reader["SensorLabel"].AsString()               
           };

    }
}
