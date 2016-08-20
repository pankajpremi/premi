
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
    public class ServiceFuelManagementDao : IServiceFuelManagementDao
    {
        private string _databaseName;
        private Db db;

        public ServiceFuelManagementDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a Service_EventCompartmentLevel object based on DataReader
        static Func<IDataReader, Service_EventCompartmentLevel> MakeTankLevel = reader =>
           new Service_EventCompartmentLevel
           {

               EventId = reader["eventId"].AsId(),
               TankCompartmentId = reader["tankCompartmentId"].AsInt(),
               GatewayId = reader["gatewayId"].AsInt(),
               GatewayName = reader["gatewayName"].AsString(),
               Facility = new Asset_GatewayFacility
               {
                   FacilityId = reader["facilityId"].AsInt(),
                   FacilityName = reader["facilityName"].AsString(),
                   GatewayId = reader["gatewayId"].AsInt(),
                   GatewayName = reader["gatewayName"].AsString()
               },
               FacilityId = reader["facilityId"].AsInt(),
               FacilityName = reader["facilityName"].AsString(),
               ProductLabel = reader["systemProductName"].AsString(),
               TankCompartmentNumber = reader["assetNumber"].AsInt(),
               TankCompartmentLabel = reader["assetName"].AsString(),
               ReadingDateTime = reader["readingDateTime"].AsDateTime(),
               AtgId = reader["atgId"].AsInt(),
               CapacityAmt = reader["capacityAmt"].AsDecimal(),
               HeightAmt = reader["heightAmt"].AsDecimal(),
               UllageAmt = reader["ullageAmt"].AsDecimal(),
               VolumeAmt = reader["volumeAmt"].AsDecimal(),
               WaterHeightAmt = reader["waterHeightAmt"].AsDecimal(),
               ReadingDateInt = reader["readingDateInt"].AsInt(),
               ReadingTimeInt = reader["readingTimeInt"].AsInt(),
               PercentVolumeAmt = (reader["volumeAmt"].AsDecimal() / (reader["capacityAmt"].AsDecimal() == 0 ? 1 : reader["capacityAmt"].AsDecimal())) * 100

           };

        // creates a Service_EventCompartmentDelivery object based on DataReader
        static Func<IDataReader, Service_EventCompartmentDelivery> MakeTankDelivery = reader =>
           new Service_EventCompartmentDelivery
           {

               EventId = reader["eventId"].AsId(),
               TankCompartmentId = reader["tankCompartmentId"].AsInt(),
               GatewayId = reader["gatewayId"].AsInt(),
               GatewayName = reader["gatewayName"].AsString(),
               Facility = new Asset_GatewayFacility
               {
                   FacilityId = reader["facilityId"].AsInt(),
                   FacilityName = reader["facilityName"].AsString(),
                   GatewayId = reader["gatewayId"].AsInt(),
                   GatewayName = reader["gatewayName"].AsString()
               },
               FacilityId = reader["facilityId"].AsInt(),
               FacilityName = reader["facilityName"].AsString(),
               ProductLabel = reader["systemProductName"].AsString(),
               TankCompartmentNumber = reader["assetNumber"].AsInt(),
               TankCompartmentLabel = reader["assetName"].AsString(),
               AtgId = reader["atgId"].AsInt(),
               DeliveredDateTime = reader["deliveredDateTime"].AsDateTime(),
               DeliveredMeasAmt = reader["deliveredMeasAmt"].AsDecimal(),
               DeliveredVolAmt = reader["deliveredVolAmt"].AsDecimal()

           };

        public List<Service_TankCompartmentLevelGroup> GetDashboardTankLevels(ApplicationSystemUser user, string Gateways, string Facilities, string Products, int? PercentLevel, int Seed, int Limit)
        {
            string storedProcName = "Service_getTankCompartmentLevel_Latest";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
            var intProductList = DbHelpers.MakeParamIntList(Products);
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

            List<SqlDataRecord> ProductList = DbHelpers.MakeParamRecordList(intProductList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@Gateways", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@Facilities",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@Products", SqlDbType = SqlDbType.Structured, Value = ProductList },
                new SqlParameter(){ ParameterName="@PageNum", SqlDbType = SqlDbType.Int, Value = Seed },
                new SqlParameter(){ ParameterName="@RecordsPerPage", SqlDbType = SqlDbType.Int, Value = Limit }
            };

            IEnumerable<Service_EventCompartmentLevel> result = db.Read(storedProcName, MakeTankLevel, parameters);

            if (result != null && result.Count() > 0)
            {
                if (PercentLevel != null)
                {
                    result = from r in result where r.PercentVolumeAmt <= PercentLevel select r;
                }

                var AvgResults = result.GroupBy(i => i.Facility.FacilityId)
                          .Select(g => new Service_TankCompartmentLevelGroup
                          {
                              FacilityId = g.Max(i => i.FacilityId),
                              GatewayId = g.Max(i => i.GatewayId),
                              GatewayName = g.Max(i => i.GatewayName),
                              FacilityName = g.Max(i => i.FacilityName),
                              NumOfTanks = g.Count(),
                              MinLevelPercent = g.Min(i => i.PercentVolumeAmt),
                              AvgLevelPercent = g.Average(i => i.PercentVolumeAmt),
                              AvgWater = g.Average(i => i.WaterHeightAmt),
                              MaxWater = g.Max(i => i.WaterHeightAmt)
                          }).ToList();


                return AvgResults;
            }

            return null;

        }

        public List<Service_EventCompartmentLevel> GetFacilityCompartmentTankLevels_Latest(ApplicationSystemUser user, int? FacilityId)
        {
            string storedProcName = "Service_getTankCompartmentLevel_Latest";

            var intGatewayList = DbHelpers.MakeParamIntList(null);
            var intFacilityList = DbHelpers.MakeParamIntList(FacilityId.ToString());
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
                new SqlParameter(){ ParameterName="@Products", SqlDbType = SqlDbType.Structured, Value = null },
                new SqlParameter(){ ParameterName="@PageNum", SqlDbType = SqlDbType.Int, Value = 0 },
                new SqlParameter(){ ParameterName="@RecordsPerPage", SqlDbType = SqlDbType.Int, Value = 0 }
            };

            IEnumerable<Service_EventCompartmentLevel> result = db.Read(storedProcName, MakeTankLevel, parameters);

            if (result != null && result.Count() > 0)
            {
                var listResult = result.ToList();

                for (int i = 0; i < listResult.Count(); i++)
                {
                    var deliveries = GetCompartmentTankDeliveries_Latest(user, listResult[i].TankCompartmentId);

                    if (deliveries != null && deliveries.Count > 0)
                    {
                        listResult[i].LatestDelivery = deliveries.FirstOrDefault();
                    }
                }

                return listResult;
            }

            return null;
        }

        public List<Service_EventCompartmentLevel> GetCompartmentTankLevels(ApplicationSystemUser user, string Gateways, string Facilities, string Products, int? TankCompartmentId, int? Interval, int Seed, int Limit)
        {
            string storedProcName = "Service_getTankCompartmentLevel";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
            var intProductList = DbHelpers.MakeParamIntList(Products);
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

            List<SqlDataRecord> ProductList = DbHelpers.MakeParamRecordList(intProductList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@Gateways", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@Facilities",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@Products", SqlDbType = SqlDbType.Structured, Value = null },
                new SqlParameter(){ ParameterName="@TankCompartmentId", SqlDbType = SqlDbType.Int, Value = TankCompartmentId },
                new SqlParameter(){ ParameterName="@Interval", SqlDbType = SqlDbType.Int, Value = Interval },
                new SqlParameter(){ ParameterName="@PageNum", SqlDbType = SqlDbType.Int, Value = Seed },
                new SqlParameter(){ ParameterName="@RecordsPerPage", SqlDbType = SqlDbType.Int, Value = Limit }
            };

            IEnumerable<Service_EventCompartmentLevel> result = db.Read(storedProcName, MakeTankLevel, parameters);

            if (result != null && result.Count() > 0)
            {
                return result.ToList();
            }

            return null;
        }

        public List<Service_EventCompartmentDelivery> GetCompartmentTankDeliveries(ApplicationSystemUser user, string Gateways, string Facilities, string Products, int? TankCompartmentId, int? Interval, int Seed, int Limit)
        {
            string storedProcName = "Service_getTankCompartmentInventory";

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intFacilityList = DbHelpers.MakeParamIntList(Facilities);
            var intProductList = DbHelpers.MakeParamIntList(Products);
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

            List<SqlDataRecord> ProductList = DbHelpers.MakeParamRecordList(intProductList, "Id");

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@Gateways", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@Facilities",SqlDbType = SqlDbType.Structured, Value = FacilityList },
                new SqlParameter(){ ParameterName="@Products", SqlDbType = SqlDbType.Structured, Value = null },
                new SqlParameter(){ ParameterName="@TankCompartmentId", SqlDbType = SqlDbType.Int, Value = TankCompartmentId },
                new SqlParameter(){ ParameterName="@Interval", SqlDbType = SqlDbType.Int, Value = Interval },
                new SqlParameter(){ ParameterName="@PageNum", SqlDbType = SqlDbType.Int, Value = Seed },
                new SqlParameter(){ ParameterName="@RecordsPerPage", SqlDbType = SqlDbType.Int, Value = Limit }
            };

            IEnumerable<Service_EventCompartmentDelivery> result = db.Read(storedProcName, MakeTankDelivery, parameters);

            if (result != null && result.Count() > 0)
            {
                return result.ToList();
            }

            return null;
        }

        public List<Service_EventCompartmentDelivery> GetCompartmentTankDeliveries_Latest(ApplicationSystemUser user, int? TankCompartmentId)
        {
            string storedProcName = "Service_getTankCompartmentInventory_Latest";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@tankCompartmentID", SqlDbType = SqlDbType.Int, Value = TankCompartmentId }
            };

            IEnumerable<Service_EventCompartmentDelivery> result = db.Read(storedProcName, MakeTankDelivery, parameters);

            if (result != null && result.Count() > 0)
            {
                return result.ToList();
            }

            return null;
        }

        public List<Service_TankCompartmentEvent> GetCompartmentTankEvents(ApplicationSystemUser user, int? TankCompartmentId, string EventTypes, int Seed, int Limit)
        {
            int interval = 50;

            var intEventTypeList = DbHelpers.MakeParamIntList(EventTypes);
            List<Service_TankCompartmentEvent> eventList = new List<Service_TankCompartmentEvent>();

            bool hasLevels = intEventTypeList.Any(cus => cus == 1);

            if (hasLevels)
            {
                var levelList = GetCompartmentTankLevels(user, null, null, null, TankCompartmentId, null, 0, interval);

                if (levelList != null && levelList.Count() > 0)
                {
                    foreach (var l in levelList)
                    {
                        var el = new Service_TankCompartmentEvent
                        {
                            EventDateTime = l.ReadingDateTime,
                            EventId = l.EventId,
                            HeightAmt = l.HeightAmt,
                            VolumeAmt = l.VolumeAmt,
                            TankCompartmentId = l.TankCompartmentId,
                            TypeId = 1,
                            TypeName = "Inventory"
                        };

                        eventList.Add(el);
                    }
                }
            }

            bool hasDeliveries = intEventTypeList.Any(cus => cus == 2);

            if (hasDeliveries)
            {
                var deliveryList = GetCompartmentTankDeliveries(user, null, null, null, TankCompartmentId, null, 0, interval);

                if (deliveryList != null && deliveryList.Count() > 0)
                {
                    foreach (var d in deliveryList)
                    {
                        var ed = new Service_TankCompartmentEvent
                        {
                            EventDateTime = d.DeliveredDateTime,
                            EventId = d.EventId,
                            HeightAmt = d.DeliveredMeasAmt,
                            VolumeAmt = d.DeliveredVolAmt,
                            TankCompartmentId = d.TankCompartmentId,
                            TypeId = 2,
                            TypeName = "Delivery"
                        };

                        eventList.Add(ed);
                    }
                }
            }

            var sortedList = eventList.OrderByDescending(x => x.EventDateTime).ToList();

            return sortedList;
        }

    }
}
