using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class SystemCountyDaoTest
    {
        LocationDao dao = null;
        string dbName = string.Empty;

        [SetUp]
        public void Setup()
        {
            dbName = "Apec2DatabaseJMM";
            dao = new LocationDao(dbName);
        }


        [Test]
        public void GetCounty_ReturnsAllCounties()
        {
            int? CountyId = null;
            string StateCode = null;

            List<System_County> county = dao.GetCounty(CountyId, StateCode);

            Assert.IsNotNull(county);
            Assert.Greater(county.Count, 0);

        }

        [Test]
        public void GetCounty_ReturnsCountyByStateCode()
        {
            int? CountyId = null;
            string StateCode = "IL";

            List<System_County> county = dao.GetCounty(CountyId, StateCode);

            Assert.IsNotNull(county);
           

        }


        [Test]
        public void GetCounty_ReturnsCountyById()
        {
            int? CountyId = 691;
            string StateCode = "IL";

            List<System_County> county = dao.GetCounty(CountyId, StateCode);

            Assert.IsNotNull(county);
            Assert.AreEqual(1, county.Count);

        }

        [TearDown]
        public void TearDown()
        {
            dao = null;
        }
    }
}
