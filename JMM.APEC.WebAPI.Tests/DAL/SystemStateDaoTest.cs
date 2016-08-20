using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class SystemStateDaoTest
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
        public void GetState_ReturnsAllStates()
        {
            string CountryCode = null;
            string StateCode = null;

            List<System_State> state = dao.GetState(CountryCode, StateCode);

            Assert.IsNotNull(state);
            Assert.Greater(state.Count, 0);

        }

        [Test]
        public void GetState_ReturnsStatesByCountryCode()
        {
            string CountryCode = "US";
            string StateCode = null;

            List<System_State> state = dao.GetState(CountryCode, StateCode);

            Assert.IsNotNull(state);
            Assert.Greater(state.Count, 0);


        }

        [Test]
        public void GetState_ReturnsStateByStateCode()
        {
            string CountryCode = "US";
            string StateCode = "FL";

            List<System_State> state = dao.GetState(CountryCode, StateCode);

            Assert.IsNotNull(state);
            Assert.AreEqual(1,state.Count);


        }

        [TearDown]
        public void TearDown()
        {
            dao = null;
        }
    }
}
