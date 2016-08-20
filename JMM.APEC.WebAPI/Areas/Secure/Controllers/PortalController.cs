using JMM.APEC.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JMM.APEC.ActionService;
using JMM.APEC.WebAPI.Models;
using JMM.APEC.Core;
using JMM.APEC.WebAPI.Areas.Admin.Models;
//using JMM.APEC.BusinessObjects.Entities;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/portals")]
    public class PortalController : SecureApiController
    {
        IService service { get; set; }

        public PortalController()
        {
            service = new Service(CurrentApiUser);
        }

        public PortalController(IService service) { this.service = service; }

        private List<AssetPortalDto> ListPortals(List<Asset_Portal> portalList)
        {
            var portals = from t in portalList
                           select new AssetPortalDto()
                           {
                               PortalId = t.Id,
                               PortalName = t.Name,
                               DomainUrl = t.DomainUrl,
                               Active = t.Active                               
                           };

            return portals.ToList();
        }

        private List<AssetGatewayDto> ListGateways(List<Asset_Gateway> gatewayList)
        {
            var gateways = from t in gatewayList
                           select new AssetGatewayDto()
                           {
                               Id = t.Id,
                               Code = t.Code,
                               Name = t.Name,
                               IdentityId = t.IdentityId
                           };

            return gateways.ToList();
        }

        private List<ModuleDto> ListModules(List<Ecomm_Module> moduleList)
        {
            var modules = from t in moduleList
                           select new ModuleDto()
                           {
                               ModuleCode = t.Code,
                               ModuleName = t.Name,
                               Services = (from s in t.Services select new ServiceDto
                               {
                                   ServiceCode = s.Code,
                                   ServiceName = s.Name

                               }).ToList()
                           };

            return modules.ToList();
        }

        [Route()]
        [Authorize]
        [HttpGet]
        public IEnumerable<AssetPortalDto> GetPortalList()
        {
            List<Asset_Portal> portalList = null;
            portalList = service.GetPortal(null, null);

            if (portalList != null)
            {
                var portals = ListPortals(portalList);

                return portals.AsEnumerable();
            }

            return null;
        }

        [Route("{portalId:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetPortal(int portalId)
        {
            List<Asset_Portal> portalList = null;
            portalList = service.GetPortal(portalId, null);

            if (portalList != null)
            {
                var portals = ListPortals(portalList);

                return Ok(portals.FirstOrDefault());
            }

            return NotFound();
        }

        [Route("{portalUrl}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetPortal(string portalUrl)
        {
            List<Asset_Portal> portalList = null;
            portalList = service.GetPortal(1, null);

            if (portalList != null)
            {
                var portals = ListPortals(portalList);

                return Ok(portals.FirstOrDefault());
            }

            return NotFound();
        }

        [Route("{portalId:int}/{Active:bool}")]
        [Authorize]
        [HttpGet]
        public AssetPortalDto GetPortalList(int portalId, bool active)
        {
            List<Asset_Portal> portalList = null;
            portalList = service.GetPortal(portalId, active);

            if (portalList != null)
            {
                var portals = ListPortals(portalList);

                return portals.FirstOrDefault();
            }

            return null;
        }

        [Route("{Active:bool}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<AssetPortalDto> GetPortalList(bool active)
        {
            List<Asset_Portal> portalList = null;
            portalList = service.GetPortal(null, active);

            if (portalList != null)
            {
                var portals = ListPortals(portalList);

                return portals.AsEnumerable();
            }

            return null;
        }

        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN")]
        [HttpPost]
        public HttpResponseMessage SavePortalList(CreatePortalBindingModel[] portals)
        {
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            List<Asset_Portal> portalList = new List<Asset_Portal>();
            foreach (var portal in portals)
            {
               var p = new Asset_Portal();
               p.Id = portal.Id; // insert -0
               p.Name = portal.Name;
               p.DomainUrl = portal.Url;
               p.Active = portal.IsActive;

               portalList.Add(p);
            }

            service.SavePortal(portalList);

            response = Request.CreateResponse(HttpStatusCode.OK, portalList);
          
            return response;
        }

        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN")]
        [HttpDelete]
        public HttpResponseMessage DeletePortalList(CreatePortalBindingModel[] portals)
        {
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            List<Asset_Portal> portalList = new List<Asset_Portal>();
            foreach (var portal in portals)
            {
                var p = new Asset_Portal();
                p.Id = portal.Id;
                p.Name = portal.Name;
                p.DomainUrl = portal.Url;
                p.Active = portal.IsActive;

                portalList.Add(p);
            }

            service.DeletePortal(portalList);

            response = new HttpResponseMessage(HttpStatusCode.NoContent);
            return response;
        }

        [Route("modules")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetModulesForUser()
        {
            List<Ecomm_Module> moduleList = null;
            moduleList = service.GetModulesForUser();

            if (moduleList != null)
            {
                var modules = ListModules(moduleList);

                return Ok(new MetadataWrapper<ModuleDto>(modules));
            }

            return NotFound();
        }

        [Route("gateways")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetGatewaysForUser()
        {
            List<Asset_Gateway> gatewayList = null;
            gatewayList = service.GetGatewaysForUser();

            if (gatewayList != null)
            {
                var gateways = ListGateways(gatewayList);

                return Ok(new MetadataWrapper<AssetGatewayDto>(gateways));
            }

            return NotFound();

        }

    }
}
