using JMM.APEC.ActionService;
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
    [RoutePrefix("api/v1/admins/users/roles")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
    public class RoleAdminController : SecureApiController
    {
        IIdentityService service { get; set; }

        public RoleAdminController()
        {
            service = new IdentityService(CurrentApiUser);
        }

        private List<RoleDto> ListRoles(List<Role> RoleList)
        {

                var Roles = from r in RoleList
                            select new RoleDto()
                            {
                                RoleId = r.Id,
                                Name = r.Name
                            };

                return Roles.ToList();
        }

        [Route("")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetRoleList()
        {
            List<Role> RoleList = null;

            RoleList = service.GetRoles();

            if (RoleList != null)
            {
                var Rolese = ListRoles(RoleList);

                return Ok(new MetadataWrapper<RoleDto>(Rolese));
            }

            return NotFound();
        }
    }
}
