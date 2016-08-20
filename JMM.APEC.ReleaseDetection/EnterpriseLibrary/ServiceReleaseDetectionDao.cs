
using JMM.APEC.Core;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.ReleaseDetection.Interfaces;
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

namespace JMM.APEC.ReleaseDetection.EnterpriseLibrary
{
    public class ServiceReleaseDetectionDao : IServiceReleaseDetectionDao
    {
        private string _databaseName;
        private Db db;

        public ServiceReleaseDetectionDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }


        public List<Service_ReleaseDetection> GetReleaseDetectionResults(ApplicationSystemUser user, string Gateways, string Facilities, 
            string Results, string RDStatuses, string AssetTypes, DateTime? Fromdate, DateTime? Todate, int? ReleaseDetectionId, int Seed, int Limit)
        {
            string storedProcName = "Service_getReleaseDetection";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
            var intResultList = DbHelpers.MakeParamIntList(Results);
            var intRDStatusList = DbHelpers.MakeParamIntList(RDStatuses);
            var intAssetTypeList = DbHelpers.MakeParamIntList(AssetTypes);

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
            List<SqlDataRecord> ResultList = DbHelpers.MakeParamRecordList(intResultList, "Id");
            List<SqlDataRecord> RdStatusList = DbHelpers.MakeParamRecordList(intRDStatusList, "Id");
            List<SqlDataRecord> AssetTypeList = DbHelpers.MakeParamRecordList(intAssetTypeList, "Id");


            var parameters = new[]{
                new SqlParameter(){ ParameterName="@gatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@facilityIDs",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@resultIDs", SqlDbType = SqlDbType.Structured, Value = ResultList },
                new SqlParameter(){ ParameterName="@rdStatusIDs",SqlDbType = SqlDbType.Structured, Value = RdStatusList },
                new SqlParameter(){ ParameterName="@assetTypeIDs", SqlDbType = SqlDbType.Structured, Value = AssetTypeList },
                new SqlParameter(){ ParameterName="@inReleaseDetectionID", SqlDbType = SqlDbType.Int, Value = ReleaseDetectionId },
                new SqlParameter(){ ParameterName="@inDateFrom", DbType = DbType.DateTime, Value = Fromdate.HasValue ? Fromdate : null },
                new SqlParameter(){ ParameterName="@inDateTo", DbType = DbType.DateTime, Value = Todate.HasValue ? Todate : null }

            };

            IEnumerable<Service_ReleaseDetection> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " User '{0}', Gateways '{1}', Facilities '{2}', Results '{3}' ,DateFrom '{4}', DateTo '{5}'.", user.UserName, Gateways, Facilities, Results, Fromdate, Todate);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

        static Func<IDataReader, Service_ReleaseDetection> Make = reader =>
           new Service_ReleaseDetection
           {
               ReleaseDetectionId = reader["ReleaseDetectionId"].AsId(),
               GatewayName = reader["GatewayName"].AsString(),
               AtgId = reader["AtgId"].AsInt(),
               AtgName = reader["AtgName"].AsString(),
               GatewayId = reader["GatewayID"].AsInt(),
               FacilityId = reader["FacilityID"].AsInt(),
               FacilityName = reader["FacilityName"].AsString(),
               Asset = new Service_ReleaseDetectionAsset
               {
                   AssetId = reader["AssetId"].AsInt(),
                   AssetLabel = reader["AssetLabel"].AsString(),
                   AssetProperty1Label = reader["AssetProperty1Label"].AsString(),
                   AssetProperty1Value = reader["AssetProperty1Value"].AsString(),
                   AssetProperty2Label = reader["AssetProperty2Label"].AsString(),
                   AssetProperty2Value = reader["AssetProperty2Value"].AsString(),
                   AssetTypeId = reader["AssetTypeId"].AsInt(),
                   AssetTypeName = reader["AssetTypeName"].AsString()
               },
               Result = new Service_ReleaseDetectionResult
               {
                   ReportDate = reader["ReportDate"].AsDateTime(),
                   ResultId = reader["ResultId"].AsInt(),
                   ResultName = reader["ResultName"].AsString(),
                   TestDate = reader["TestDate"].AsDateTime(),
                   TestDateTimeZone = reader["TestDateTimeZone"].AsString(),
                   TestResultId = reader["TestResultId"].AsInt(),
                   TestResultName = reader["TestResultName"].AsString(),
                   TestTypeId = reader["TestTypeId"].AsInt(),
                   TestTypeLabel = reader["TestTypeLabel"].AsString(),
               },
               RdStatus = new Service_ReleaseDetectionStatus
               {
                   RdStatusId = reader["RdStatusId"].AsInt(),
                   RdStatusName = reader["RdStatusName"].AsString()
               },
               Template = new Service_ReleaseDetectionAssetTemplate
               {
                   EffectiveStartDate = reader["EffectiveStartDate"].AsDateTime(),
                   EffectiveEndDate = reader["EffectiveEndDate"].AsDateTime(),
                   MethodName = reader["MethodName"].AsString(),
                   Primary = true,
                   TemplateId = 1,
                   TemplateName = "15th"
               }
           };
    }
}
