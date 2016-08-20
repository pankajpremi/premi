//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
//using JMM.APEC.DataObjects.Interfaces;
using JMM.APEC.Core;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.IMS.Interfaces;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS.EnterpriseLibrary
{
    public class ServiceFlowMeterDao : IServiceFlowMeterDao
    {
        private string _databaseName;
        private Db db;

        public ServiceFlowMeterDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        static Func<IDataReader, Service_FlowMeter> MakeTankLevel = reader =>
           new Service_FlowMeter
           {
               AvgGPMValue = reader["AvgGPM"].AsDecimal(),
               FacilityId = reader["FacilityID"].AsInt(),
               FacilityName = reader["FacilityName"].AsString(),
               FlowMeterNumber = reader["FlowMeterNumber"].AsString(),
               FuelPositionId = reader["FuelPositionID"].AsInt(),
               FuelPositionLabel = reader["FuelPositionLabel"].AsString(),
               FuelPositionNumber = reader["FuelPositionNumber"].AsString(),
               HoseNumber = reader["HoseNumber"].AsString(),
               NumOfTransactions = reader["NumOfTransactions"].AsInt(),
               ReadingDateTime = reader["ReadingDate"].AsDateTime(),
               TotalVolumeAmt = 0.00M,
               EventId = reader["EventId"].AsId(),
               GatewayId = reader["gatewayId"].AsInt(),
               GatewayName = reader["gatewayName"].AsString(),

           };

        public List<Service_FlowMeterGroup> GetDashboardFlowMeters(ApplicationSystemUser user, string Gateways, string Facilities, decimal? GMPLevel, int Seed, int Limit)
        {
            string storedProcName = "Service_getFlowMeters_Latest";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
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
        
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@Gateways", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@Facilities",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@PageNum", SqlDbType = SqlDbType.Int, Value = Seed },
                new SqlParameter(){ ParameterName="@RecordsPerPage", SqlDbType = SqlDbType.Int, Value = Limit }
            };

            IEnumerable<Service_FlowMeter> result = db.Read(storedProcName, MakeTankLevel, parameters);

            if (result != null && result.Count() > 0)
            {

                var AvgResults = result.GroupBy(i => i.FacilityId)
                          .Select(g => new Service_FlowMeterGroup
                          {
                              FacilityId = g.Max(i => i.FacilityId),
                              GatewayId = g.Max(i => i.GatewayId),
                              GatewayName = g.Max(i => i.GatewayName),
                              FacilityName = g.Max(i => i.FacilityName),
                              NumOfDispensers = g.Count(),
                              NumOfTransactions = g.Sum(i => i.NumOfTransactions),
                              AvgGPM = g.Average(i => i.AvgGPMValue),
                              MinGPM = g.Min(i => i.AvgGPMValue)
                          }).ToList();


                return AvgResults;
            }

            return null;
        }

        public List<Service_FlowMeter> GetFacilityFlowMeters_Latest(ApplicationSystemUser user, int? FacilityId)
        {
            List<Service_FlowMeter> meters = new List<Service_FlowMeter>();

            var m1 = new Service_FlowMeter
            {
                FuelPositionId = 1,
                FuelPositionLabel = "01",
                FuelPositionNumber = "00",
                FlowMeterNumber = "01",
                HoseNumber = "00",
                AvgGPMValue = 5.47M,
                NumOfTransactions = 45,
                ReadingDateTime = new DateTime(2015, 10, 02),
                EventId = 1,
                TotalVolumeAmt = 456
            };

            var m2 = new Service_FlowMeter
            {
                FuelPositionId = 2,
                FuelPositionLabel = "02",
                FuelPositionNumber = "01",
                FlowMeterNumber = "01",
                HoseNumber = "01",
                AvgGPMValue = 5.63M,
                NumOfTransactions = 36,
                ReadingDateTime = new DateTime(2015, 10, 02),
                EventId = 2,
                TotalVolumeAmt = 345
            };

            meters.Add(m1);
            meters.Add(m2);

            return meters;
        }

        public List<Service_FlowMeter> GetFuelPositionFlowMeters(ApplicationSystemUser user, string Gateways, string Facilities, int? FuelPositionId, int? Interval, int Seed, int Limit)
        {
            List<Service_FlowMeter> meters = new List<Service_FlowMeter>();

            var m1 = new Service_FlowMeter
            {
                FuelPositionId = 1,
                FuelPositionLabel = "01",
                FuelPositionNumber = "00",
                FlowMeterNumber = "01",
                HoseNumber = "00",
                AvgGPMValue = 5.47M,
                NumOfTransactions = 45,
                ReadingDateTime = new DateTime(2015, 10, 02),
                EventId = 1,
                TotalVolumeAmt = 456
            };

            var m2 = new Service_FlowMeter
            {
                FuelPositionId = 2,
                FuelPositionLabel = "02",
                FuelPositionNumber = "01",
                FlowMeterNumber = "01",
                HoseNumber = "01",
                AvgGPMValue = 5.63M,
                NumOfTransactions = 36,
                ReadingDateTime = new DateTime(2015, 10, 02),
                EventId = 2,
                TotalVolumeAmt = 345
            };

            meters.Add(m1);
            meters.Add(m2);

            return meters;
        }
        public List<Service_FlowMeterEvent> GetFlowMeterEvents(ApplicationSystemUser user, int? FuelPositionId, int Seed, int Limit)
        {
            List<Service_FlowMeterEvent> meters = new List<Service_FlowMeterEvent>();

            var m1 = new Service_FlowMeterEvent
            {
                AvgGPMValue = 5.47M,
                NumOfTransactions = 45,
                ReadingDateTime = new DateTime(2015, 10, 02),
                TotalVolumeAmt = 456
            };

            var m2 = new Service_FlowMeterEvent
            {
                AvgGPMValue = 5.63M,
                NumOfTransactions = 36,
                ReadingDateTime = new DateTime(2015, 10, 02),
                TotalVolumeAmt = 345
            };

            meters.Add(m1);
            meters.Add(m2);

            return meters;
        }
    }
}
