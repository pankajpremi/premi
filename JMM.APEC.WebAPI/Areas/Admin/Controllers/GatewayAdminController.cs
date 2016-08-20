using JMM.APEC.ActionService;
using JMM.APEC.Core;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.WebAPI.Areas.Admin.Models;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Admin.Controllers
{
    [RoutePrefix("api/v1/admins/gateways")]
    public class GatewayAdminController : SecureApiController
    {
        IService service { get; set; }
        IdentityService identService { get; set; }

        public GatewayAdminController()
        {
            service = new Service(CurrentApiUser);
            identService = new IdentityService(CurrentApiUser);
        }

        public GatewayAdminController(IService service) { this.service = service; }

        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPost]
        public IHttpActionResult SaveGateway([FromBody] CreateGatewayBindingModel g)
        {
            if (g == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gateway = new Asset_Gateway();
            gateway.Id = 0; 
            gateway.Name = g.GatewayName;
            gateway.ShortName = g.GatewayShortName;
            gateway.StatusId = g.StatusId;
            gateway.EffectiveEndDate = g.EffectiveEndDate;
            gateway.AppChangeUserId = g.AppChangeUserId;

            var gtway = service.SaveGateway(gateway);

            if (gtway.IsValid())
            {
                return Ok(gtway);
            }
            else
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }

        }


        [Route("{gatewayid:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPut]
        public IHttpActionResult UpdateGateway(int gatewayId, [FromBody]CreateGatewayBindingModel g)
        {
            if (g == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (gatewayId < 0)
            {
                return BadRequest();
            }

         
            var gateway = new Asset_Gateway();
            gateway.Id = gatewayId; // input id
            gateway.Name = g.GatewayName;
            gateway.ShortName = g.GatewayShortName;
            gateway.StatusId = g.StatusId;
            gateway.EffectiveEndDate = g.EffectiveEndDate.HasValue ? g.EffectiveEndDate : null;
            gateway.AppChangeUserId = g.AppChangeUserId;

            var gtway = service.SaveGateway(gateway);

            if (gtway.IsValid())
            {
                return Ok(gtway);
            }
            else
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }

        }

        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpDelete]
        public HttpResponseMessage DeleteGateway(int gatewayid,CreateGatewayBindingModel g)
        {
            HttpResponseMessage response = null;
            if (gatewayid <= 0)
            {
                Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            if (g == null)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, Resources.LangResource.NullInput);
            }

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

        
            var gateway = new Asset_Gateway();
            gateway.Id = gatewayid;
            gateway.Name = g.GatewayName;
            gateway.ShortName = g.GatewayShortName;
            gateway.StatusId = g.StatusId;
            gateway.EffectiveEndDate = g.EffectiveEndDate;
            gateway.AppChangeUserId = g.AppChangeUserId;

            service.DeleteGateway(gateway);

            response = Request.CreateResponse(HttpStatusCode.OK, gateway);
            return response;
        }


        [Route("{gatewayid:int}/locations")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPut]
        public IHttpActionResult SaveGatewayLocation(int GatewayId,[FromBody]AssetGatewayLocationBindingModel glocations)
        {
            HttpResponseMessage response = null;

            if (GatewayId <= 0)
            {
                return BadRequest();
            }

            if (glocations == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                    return BadRequest(ModelState);
            }

                List<Asset_GatewayLocation> gatewaylocations = new List<Asset_GatewayLocation>();
         
                var gatewayLoc = new Asset_GatewayLocation();
                gatewayLoc.GatewayId = GatewayId;
                gatewayLoc.Address1 = glocations.Address1;
                gatewayLoc.Address2 = glocations.Address2;
                gatewayLoc.City = glocations.City;
                gatewayLoc.PostalCode = glocations.PostalCode;
                gatewayLoc.StateCode = glocations.StateCode;
                gatewayLoc.CountryCode = glocations.CountryCode;
                gatewayLoc.Email = glocations.Email;
              
                gatewayLoc.Phones = glocations.phones;               
            

            gatewayLoc.AppChangeUserId = glocations.AppChangeUserId;
            gatewaylocations.Add(gatewayLoc);

           if( service.SaveGatewayLocation(gatewaylocations) < 0)
            {
                    return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }
           else
            {
                    return Ok();
            }
            
            
        }

        [Route("{gatewayid:int}/systemroles")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpGet]
        public IHttpActionResult GetGatewaySystemRoles(int GatewayId)
        {
            List<GatewaySystemRole> RoleList = null;
            RoleList = identService.GetSystemRolesForGateway(CurrentApiUser.Portal.PortalId, GatewayId);

            if (RoleList != null & RoleList.Count > 0)
            {
                var Roles = ListSystemRoles(RoleList);

                return Ok(new MetadataWrapper<GatewaySystemRoleDto>(Roles));
            }

            return NotFound();
        }

        private List<GatewaySystemRoleDto> ListSystemRoles(List<GatewaySystemRole> RoleList)
        {
            var Roles = from t in RoleList
                        select new GatewaySystemRoleDto()
                        {
                            RoleId = t.RoleId,
                            Name = t.RoleName,
                            Code = t.RoleCode,
                            GatewayId = t.GatewayPortalId,
                            Permissions = (from p in t.Permissions select new PermissionDto
                            {
                                Code = p.Code,
                                Name = p.Name,
                                PermissionId = p.PermissionId
                            }).ToList()

                        };
            return Roles.ToList();

        }


    }
}
