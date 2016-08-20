using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class AssetGatewayDaoTest
    {
        AssetGatewayDao dao = null;
        string dbName = string.Empty;

        [SetUp]
        public void Setup()
        {
            dbName = "Apec2DatabaseJMM";
            dao = new AssetGatewayDao(dbName);
        }


        [Test]
        public void GetGateway_ReturnsAllGateways()
        {
            int? portalId = null;
            bool? isPortalActive = null;
            int? GatewayId = null;
            string gatewaycode = null;
            int? statusId = null;


            List<Asset_Gateway> gatewaylist = dao.GetGateway(portalId, isPortalActive, GatewayId, gatewaycode, statusId);

            Assert.AreEqual(2, gatewaylist.Count);

        }


        [Test]
        public void GetGateway_ReturnsAllGatewaysForPortal()
        {
            int? portalId = 1;
            bool? isPortalActive = true;
            int? GatewayId = null;
            string gatewaycode = null;
            int? statusId = null;

            List<Asset_Gateway> gatewaylist = dao.GetGateway(portalId, isPortalActive, GatewayId, gatewaycode, statusId);

            Assert.AreEqual(2, gatewaylist.Count);

        }

        [Test]
        public void GetGetwayById_ReturnsGetwayForPortal()
        {
            int? portalId = 1;
            bool? isPortalActive = true;
            int? GatewayId = 4;
            string gatewaycode = null;
            int? statusId = null;

            List<Asset_Gateway> gatewaylist = dao.GetGateway(portalId, isPortalActive, GatewayId, gatewaycode, statusId);

            Assert.AreEqual(1, gatewaylist.Count);
            Assert.IsInstanceOf<Asset_Gateway>(gatewaylist[0]);
            Assert.AreNotSame("GEI", gatewaylist[0].ShortName);
        }

        [Test]
        public void GetGatewayById_ReturnsNullForInvalidId()
        {

            int? portalId = 1;
            bool? isPortalActive = true;
            int? GatewayId = 100;
            string gatewaycode = null;
            int? statusId = null;

            List<Asset_Gateway> gatewaylist = dao.GetGateway(portalId, isPortalActive, GatewayId, gatewaycode, statusId);

            Assert.IsNull(gatewaylist);
        }


        public void SaveGateway_ReturnGatewayId()
        {
            Asset_Gateway gateway = new Asset_Gateway();

            gateway.Id = 0;
            gateway.Name = "testgate";
            gateway.ShortName = "tstgt";
            gateway.StatusId = 1;
            gateway.EffectiveEndDate = null;
            gateway.AppChangeUserId = 3;

            dao.InsertGateway(gateway);

            Assert.Greater(gateway.Id, 0);
        }

        
        public void UpdateGateway()
        {
            Asset_Gateway gateway = new Asset_Gateway();

            gateway.Id = 5;
            gateway.Name = "Graham C-Stores";
            gateway.ShortName = "GCS";
            gateway.StatusId = null;
            gateway.EffectiveEndDate = null;
            gateway.AppChangeUserId = 3;

            dao.UpdateGateway(gateway);
            
        }


        public void DeleteGateway()
        {
            Asset_Gateway gateway = new Asset_Gateway();

            gateway.Id = 5;
            gateway.Name = "Graham C-Stores";
            gateway.ShortName = "GCS";
            gateway.StatusId = null;
            gateway.EffectiveEndDate = null;
            gateway.AppChangeUserId = 3;

            dao.DeleteGateway(gateway);
        }


        [TearDown]
        public void TearDown()
        {
            dao = null;
        }

    }
}
