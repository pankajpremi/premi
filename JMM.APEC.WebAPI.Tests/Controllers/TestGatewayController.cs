using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.ActionService;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.WebAPI.Areas.Secure.Controllers;
using JMM.APEC.WebAPI.Models;
using NUnit.Framework;
using Moq;



namespace JMM.APEC.WebAPI.Tests.Controllers
{
    [TestFixture]
    public class TestGatewayController
    {
        Mock<IService> mockService;

        [SetUp]
        public void InitializeMocks()
        {
            mockService = new Mock<IService>();
        }

        GatewayController CreateTestGatewayController()
        {
            // Note: this is where DI (Dependency Injection) takes place.
            // the service layer is injected (via the constructor) into the controller.
            return new GatewayController(mockService.Object);
        }
    }
}