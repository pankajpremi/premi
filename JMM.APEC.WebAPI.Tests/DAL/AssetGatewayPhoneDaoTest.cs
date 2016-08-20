using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class AssetGatewayPhoneDaoTest
    {
        AssetAddressDao dao = null;
        string dbName = string.Empty;

        [SetUp]
        public void Setup()
        {
            dbName = "Apec2DatabaseJMM";
            dao = new AssetAddressDao(dbName);
        }


        [Test]
        public void GetAddress_ReturnsAddressById()
        {
            int AddressId = 1;

            List<Asset_Address> address = dao.GetAddress(AddressId);

            Assert.IsNotNull(address);

        }

        [TearDown]
        public void TearDown()
        {
            dao = null;
        }
    }
}
