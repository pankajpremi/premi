using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class SystemCountryDaoTest
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
        public void GetCountry_ReturnsAllCountries()
        {
            string CountryCode = null;
            
            List<System_Country> country = dao.GetCountry(CountryCode);

            Assert.IsNotNull(country);
            Assert.Greater(country.Count, 0);

        }

        [Test]
        public void GetCountry_ReturnsCountryByCode()
        {
            string CountryCode = "US";

            List<System_Country> country = dao.GetCountry(CountryCode);

            Assert.IsNotNull(country);
            Assert.Greater(country.Count, 0);

        }

        [TearDown]
        public void TearDown()
        {
            dao = null;
        }
    }
}
