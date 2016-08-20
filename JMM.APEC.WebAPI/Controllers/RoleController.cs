using JMM.APEC.WebAPI.Infrastructure;
using JMM.APEC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Controllers
{
    [RoutePrefix("api/role")]
    public class RoleController : SecureApiController
    {
        [Route("{id:int}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(int Id)
        {
            var role = await this.AppRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                return Ok(AppModelFactory.Create(role));
            }
            return NotFound();
        }

        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = this.AppRoleManager.Roles;

            return Ok(roles);
        }

        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new ApplicationIdentityRole { Name = model.Name, ServiceId = model.ServiceId };

            var result = await this.AppRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, AppModelFactory.Create(role));
        }

    }
}
