using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;
using JMM.APEC.BusinessObjects;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class AssetFacilityDaoTest
    {
        AssetFacilityDao dao = null;
        string dbName = string.Empty;

        [SetUp]
        public void Setup()
        {
            dbName = "Apec2DatabaseJMM";
            dao = new AssetFacilityDao(dbName);
        }


        [Test]
        public void GetFacility_ReturnsAllFacilities_ByGateway()
        {
            int GatewayId = 4;
            int? FacilityId = null;
            int? statusId = null;
            string gatewaycode = null;
            int? gatewaystatusId = null;
            ApplicationSystemUser user = null;


            List<Asset_Facility> facilitylist = dao.GetFacility(user,GatewayId, FacilityId, statusId, gatewaycode, gatewaystatusId);

            Assert.IsNotNull(facilitylist);

        }

         [Test]
        public void GetFacilityById_ReturnsNullForInvalidId()
        {
            int GatewayId = 100;
            int? FacilityId = null;
            int? statusId = null;
            string gatewaycode = null;
            int? gatewaystatusId = null;
            ApplicationSystemUser user = null;


            List<Asset_Facility> facilitylist = dao.GetFacility(user,GatewayId, FacilityId, statusId, gatewaycode, gatewaystatusId);

            Assert.IsNull(facilitylist);
        }


        public void SaveFacility_ReturnNothing()
        {
            Asset_Facility facility = new Asset_Facility();

            facility.Id = 0;
            facility.GatewayId = 4;
            facility.Name = "NewFacility";
            facility.AKAName = "NewFac";
            facility.AddressId = null;
            facility.StatusId = null;
            facility.TypeId = null;
            facility.IsDeleted = null;
            facility.AppChangeUserId = 3;

            dao.InsertFacility(facility);

            Assert.Greater(facility.Id, 0);
        }


        public void UpdateGateway()
        {
            Asset_Facility facility = new Asset_Facility();

            facility.Id = 1;
            facility.GatewayId = 5;
            facility.Name = "GCS-203a";
            facility.AKAName = "Joliet BP_test";
            facility.AddressId = null;
            facility.StatusId = 34;
            facility.TypeId = 144;
            facility.IsDeleted = false;
            facility.AppChangeUserId = 3;

            dao.UpdateFacility(facility);

        }


        public void DeleteGateway()
        {
            Asset_Facility facility = new Asset_Facility();

            facility.Id = 1;
            facility.GatewayId = 5;
            facility.Name = "GCS-203a";
            facility.AKAName = "Joliet BP_test";
            facility.AddressId = null;
            facility.StatusId = 34;
            facility.TypeId = 144;
            facility.IsDeleted = false;
            facility.AppChangeUserId = 3;

            dao.DeleteFacility(facility);
        }


        [TearDown]
        public void TearDown()
        {
            dao = null;
        }

    }
}
