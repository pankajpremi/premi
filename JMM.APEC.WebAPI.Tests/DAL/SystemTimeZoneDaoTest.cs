using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class SystemTimeZoneDaoTest
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
        public void GetTimeZone_ReturnsAllTimeZones()
        {
            int? TimeZoneId = null;
            string TimeZoneCode = null;

            List<System_TimeZone> tz = dao.GetTimeZone(TimeZoneId, TimeZoneCode);

            Assert.IsNotNull(tz);
            Assert.Greater(tz.Count, 0);

        }

        [TearDown]
        public void TearDown()
        {
            dao = null;
        }
    }
}
