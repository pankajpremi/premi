using JMM.APEC.ActionService;
using JMM.APEC.Calendar;
//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.WebAPI.Areas.Secure.Models;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/calendars")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,EVNTRK")]
    public class CalendarController : SecureApiController
    {
        IService service { get; set; }

        public CalendarController()
        {
            service = new Service(CurrentApiUser);
        }

        public CalendarController(IService service) { this.service = service; }


        private List<ServiceCalendarToDoDto> ListResults(List<Service_CalendarToDo> datalist)
        {
            var data = from t in datalist
                          select new ServiceCalendarToDoDto()
                          {
                              EventTrackerReminderId = t.EventTrackerReminderId,
                              GatewayName = t.GatewayName,
                              ServiceName = t.ServiceName,
                              SubjectName = t.SubjectName,
                              Status = t.Status,
                              FacilityName = t.FacilityName,
                              Duedate = t.Duedate
                          };

            return data.ToList();
        }



        [Route("todo")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserToDo([FromUri]ServiceCalendarBindingModel model)
        {
            List<Service_CalendarToDo> resultList = null;

            if(model != null)
            {
                resultList = service.GetCalendarToDo(model.Gateways, model.Services, model.FromDate, model.ToDate, 1, 25);
            }
           else
            {
                resultList = service.GetCalendarToDo(null, null, null, null,0, 0);
            }

            if (resultList != null)
            {
                var results = ListResults(resultList);

                return Ok(new MetadataWrapper<ServiceCalendarToDoDto>(results));
            }

            return NotFound();
        }





    }

}
