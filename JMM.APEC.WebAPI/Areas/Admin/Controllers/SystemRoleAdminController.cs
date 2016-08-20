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
    [RoutePrefix("api/v1/admins/users/systemroles")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
    public class SystemRoleAdminController : SecureApiController
    {
        IIdentityService service { get; set; }

        public SystemRoleAdminController()
        {
            service = new IdentityService(CurrentApiUser);
        }

        private List<PermissionDto> ListPermissions(List<Permission> PermissionList)
        {
            var Permissions = from p in PermissionList
                              select new PermissionDto()
                              {
                                  Code = p.Code,
                                  Name = p.Name,
                                  PermissionId = p.PermissionId
                              };

            return Permissions.ToList();

        }

        private List<SystemRoleDto> ListSystemRoles(List<SystemRole> RoleList)
        {

            var Roles = from r in RoleList
                        select new SystemRoleDto()
                        {
                            SystemRoleId = r.ID,
                            Code = r.Code,
                            Name = r.Name,
                            ServiceCode = r.ServiceCode
                            
                        };

            return Roles.ToList();
        }

        [Route("")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetSystemRoleList()
        {
            List<SystemRole> RoleList = null;
            RoleList = service.GetSystemRoles();

            if (RoleList != null)
            {
                var Roles = ListSystemRoles(RoleList);

                return Ok(new MetadataWrapper<SystemRoleDto>(Roles));
            }

            return NotFound();
        }


        [Route("permissions")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetPermissionList()
        {
            List<Permission> PermissionList = null;
            PermissionList = service.GetPermissions();

            if (PermissionList != null)
            {
                var Roles = ListPermissions(PermissionList);

                return Ok(new MetadataWrapper<PermissionDto>(Roles));
            }

            return NotFound();
        }
    }
}
