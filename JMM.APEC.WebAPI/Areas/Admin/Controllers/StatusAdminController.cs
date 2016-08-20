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
    [RoutePrefix("api/v1/admins/users/statuses")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
    public class StatusAdminController : SecureApiController
    {
        IIdentityService service { get; set; }

        public StatusAdminController()
        {
            service = new IdentityService(CurrentApiUser);
        }

        private List<StatusDto> ListStatuses(List<Status> StatusList)
        {

            var Statuses = from s in StatusList
                        select new StatusDto()
                        {
                            StatusId = s.Id,
                            Code = s.Code,
                            Name = s.Value
                        };

            return Statuses.ToList();
        }

        [Route("")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetStatusList()
        {
            List<Status> StatusList = null;

            StatusList = service.GetStatuses();

            if (StatusList != null)
            {
                var Statuses = ListStatuses(StatusList);

                return Ok(new MetadataWrapper<StatusDto>(Statuses));
            }

            return NotFound();
        }
    }
}
