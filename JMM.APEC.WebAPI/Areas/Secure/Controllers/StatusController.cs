using JMM.APEC.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.ActionService;
using JMM.APEC.Core;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/statuses")]
    public class StatusController : SecureApiController
    {
        IService service { get; set; }

        public StatusController()
        {
            service = new Service(CurrentApiUser);
        }

        private List<SystemStatusDto> ListStatuses(List<System_StatusGateway> statusList)
        {
            var statuses = from t in statusList
                           select new SystemStatusDto()
                           {
                               GatewayId = t.GatewayId,
                               StatusId = t.Status.Id,
                               StatusCode = t.Status.Code,
                               StatusValue = t.Status.Value,
                               Description = t.Status.Description,
                               StatusTypeCode = t.Status.StatusType.Code,
                               StatusTypeName = t.Status.StatusType.Name
                           };

            return statuses.ToList();
        }

        [Route("{StatusTypeCode}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetStatus(string StatusTypeCode, [FromUri]SystemStatusBindingModel model)
        {
            List<System_StatusGateway> StatusList = null;
            if (model != null)
            {
                StatusList = service.GetStatus(model.Gateways, StatusTypeCode, model.StatusCode);
            }
            else
            {
                StatusList = service.GetStatus(null, null, null);
            }

            if (StatusList != null)
            {
                var status = ListStatuses(StatusList);
                return Ok(new MetadataWrapper<SystemStatusDto>(status));
            }

            return NotFound();
        }


        [Route("alarms/actionstatus")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAlarmActionStatus([FromUri]SystemStatusBindingModel model)
        {
            //List<System_Status> StatusList = null;
            //StatusList = service.GetAlarmActionStatus();

            List<System_StatusGateway> StatusList = null;
            if (model != null)
            {
                StatusList = service.GetStatus(model.Gateways, "ALMSTS", null);
            }
            else
            {
                StatusList = service.GetStatus(null, null, null);
            }

            if (StatusList != null)
            {

                return Ok(new MetadataWrapper<System_StatusGateway>(StatusList));
            }

            return NotFound();
        }


        [Route("alarms/resolutionstatus")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAlarmResolutionStatus([FromUri]SystemStatusBindingModel model)
        {
            List<System_Status> StatusList = null;
            StatusList = service.GetAlarmResolutionStatus();

            // List<System_Status> StatusList = null;
            //if (model != null)
            //{
            //    StatusList = service.GetSystemStatus(model.Gateways, "ALMRES", null);
            //}
            //else
            //{
            //    StatusList = service.GetSystemStatus(null, null, null);
            //}

            if (StatusList != null)
            {

                return Ok(new MetadataWrapper<System_Status>(StatusList));
            }

            return NotFound();
        }

        [Route("events/actionresults")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetEventTrackerActionResults([FromUri]SystemStatusBindingModel model)
        {
            List<System_StatusGateway> StatusList = null;
            if(model != null)
            {
                StatusList = service.GetStatus(model.Gateways, "ETACRS", null);
            }
            else
            {
                StatusList = service.GetStatus(null, null, null);
            }
            

            if (StatusList != null)
            {

                return Ok(new MetadataWrapper<System_StatusGateway>(StatusList));
            }

            return NotFound();
        }


      

    }
}
