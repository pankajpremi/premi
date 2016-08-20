using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using JMM.APEC.ActionService;
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.WebAPI.Areas.Secure.Controllers;
using JMM.APEC.WebAPI.Models;
using NUnit.Framework;
using Moq;


namespace JMM.APEC.WebAPI.Tests
{
    [TestFixture]
    public class TestPortalController
    { 
        Mock<IService> mockService;

        [SetUp]
        public void InitializeMocks()
        {
            mockService = new Mock<IService>();                        
        }

        PortalController CreateTestPortalController()
        {
            // Note: this is where DI (Dependency Injection) takes place.
            // the service layer is injected (via the constructor) into the controller.
            return new PortalController(mockService.Object);
        }


        private static IList<Asset_Portal> GetPortals()
        {
            // setup for getting a list of portals
            IList<Asset_Portal> portals = new List<Asset_Portal> { new Asset_Portal { Id = 1, Name = "JMM_portal", DomainUrl="www.jmmapec.com", Active = true },
            new Asset_Portal { Id = 2, Name = "Sheetz_portal", DomainUrl="www.sheetzapec.com", Active = true },
            new Asset_Portal { Id = 3, Name = "USPS_portal", DomainUrl="www.USPSapec.com", Active = true }
            };

            return portals;
        }


        [Test]
        public void Get_All_Returns_AllPortals()
        {

            List<Asset_Portal> portals = GetPortals().ToList();
             mockService.Setup(s => s.GetPortal(null, null)).Returns(portals);

                      
            // Arrange
            var controller = CreateTestPortalController();

            // Act
            var actionResult = controller.GetPortalList() as List<AssetPortalDto>;

            // Assert
            Assert.IsNotNull(actionResult, "Result is null");
            Assert.IsInstanceOf<List<AssetPortalDto>>(actionResult);
            Assert.AreEqual(3, actionResult.Count());
        }


        [Test]
        public void Get_CorrectPortalId_Returns_Category()
        {
            // setup for getting a list of portals
            List<Asset_Portal> portals = GetPortals().ToList();

            mockService.Setup(s => s.GetPortal(1, null)).Returns(portals);        

            // Arrange
            var controller = CreateTestPortalController();

            //Act
            var actionResult = controller.GetPortal(1) as AssetPortalDto;

            // Assert
            Assert.AreEqual("JMM_portal", actionResult.PortalName);                           

        }
        

        [Test]
        public void Post_Portal_Returns_OKStatusCode()
        {
            // This version uses a mock UrlHelper.

            Mock<IService> mockService;
            mockService = new Mock<IService>();

            // Arrange
            var controller = CreateTestPortalController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://location/";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;

            // Act
            CreatePortalBindingModel[] model = new CreatePortalBindingModel[1];

            for (int i = 0; i < model.Length; i++)
            {
                model[i] = new CreatePortalBindingModel();

                model[i].Id = 0;
                model[i].IsActive = true;
                model[i].Url = "test.com";
                model[i].Name = "test";
                model[i].AppChangeUserId = 358;
            }
                  
           var response = controller.SavePortalList(model);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //var newPortal = JsonConvert.DeserializeObject<AssetPortalDto>(response.Content.ReadAsStringAsync().Result);
            //Assert.AreEqual(string.Format("http://localhost/api/v1/portals/{0}", newPortal.PortalId), response.Headers.Location.ToString());

        }

        [Test]
        public void Post_EmptyPortal_Returns_BadRequestStatusCode()
        {
            // This version uses a mock UrlHelper.

            Mock<IService> mockService;
            mockService = new Mock<IService>();

            // Arrange
            var controller = CreateTestPortalController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://location/";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;

            // Act
            CreatePortalBindingModel[] model = new CreatePortalBindingModel[1];

            for (int i = 0; i < model.Length; i++)
            {
                model[i] = new CreatePortalBindingModel();

                model[i].Id = 0;
                model[i].IsActive = true;
                model[i].Url = "test.com";
                model[i].Name = "";
                model[i].AppChangeUserId = 358;
            }

            // The ASP.NET pipeline doesn't run, so validation don't run. 
            controller.ModelState.AddModelError("", "mock error message");

            var response = controller.SavePortalList(model);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);          

        }



    }
}
