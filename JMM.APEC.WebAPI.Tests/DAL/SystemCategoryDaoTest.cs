using System.Collections.Generic;
using NUnit.Framework;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects.EnterpriseLibrary;

namespace JMM.APEC.WebAPI.Tests.DAL
{
    [TestFixture]
    public class SystemCategoryDaoTest
    {
        SystemCategoryDao dao = null;
        string dbName = string.Empty;

        [SetUp]
        public void Setup()
        {
            dbName = "Apec2DatabaseJMM";
            dao = new SystemCategoryDao(dbName);
        }


       


        [TearDown]
        public void TearDown()
        {
            dao = null;
        }


    }
}
