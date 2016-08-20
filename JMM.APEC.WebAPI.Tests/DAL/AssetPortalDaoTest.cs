using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;
using System.Collections.Generic;

namespace JMM.APEC.WebAPI.Tests
{
    [TestFixture]
    public class AssetPortalDaoTest
    {
        AssetPortalDao dao = null;
        string dbName = string.Empty;

        [SetUp]
        public void Setup()
        {
            dbName = "Apec2DatabaseJMM";
            dao = new AssetPortalDao(dbName);
        }


        [Test]
        public void GetPortal_ReturnsAllPortals()
        {
           
            List<Asset_Portal> portallist = dao.GetPortal(null, null);

            Assert.AreEqual(2, portallist.Count);
           
        }

        [Test]
        public void GetPortalById_ReturnsPortal()
        {

            List<Asset_Portal> portallist = dao.GetPortal(1, true);

            Assert.AreEqual(1, portallist.Count);
            Assert.IsInstanceOf<Asset_Portal>(portallist[0]);
            Assert.AreNotSame("test.com", portallist[0].DomainUrl);
        }

        [Test]
        public void GetPortalById_ReturnsNull()
        {

            List<Asset_Portal> portallist = dao.GetPortal(100, true);

            Assert.IsNull(portallist);
        }


        //SavePortal
        public void SavePortal()
        {
            Asset_Portal newportal = new Asset_Portal();

            newportal.DomainUrl = "NewPortalDomain";
            newportal.Active = false;
            newportal.Name = "NewName";
            newportal.ModifiedUserId = 358;

             dao.InsertPortal(newportal);

            Assert.AreEqual(3, newportal.Id);
        }

        //updatePortal
        public void UpdatePortal()
        {
            Asset_Portal updatedportal = new Asset_Portal();

            updatedportal.Id = 3;
            updatedportal.DomainUrl = "UpdatedPortalDomain";
            updatedportal.Active = true;
            updatedportal.Name = "UpdatedName";
            updatedportal.ModifiedUserId = 358;

            dao.UpdatePortal(updatedportal);

            Assert.AreEqual(3, updatedportal.Id);
        }


        //DeletePortal
        public void DeletePortal()
        {
            Asset_Portal deleteportal = new Asset_Portal();

            deleteportal.Id = 3;
            deleteportal.DomainUrl = "UpdatedPortalDomain";
            deleteportal.Active = true;
            deleteportal.Name = "UpdatedName";
            deleteportal.ModifiedUserId = 358;

            dao.DeletePortal(deleteportal);



            
        }


        [TearDown]
        public void TearDown()
        {
            dao = null;
        }

    }
}
