using JMM.APEC.ActionService;
using JMM.APEC.WebAPI.Areas.Secure.Models;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/eventtrackers")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,EVNTRK")]
    public class EventTrackerController : SecureApiController
    {
        IService service { get; set; }

        public EventTrackerController()
        {
            service = new Service(CurrentApiUser);
        }

        public EventTrackerController(IService service)
        {
           this.service = service;
        }

        private List<ServiceEventTrackerDto> ListEvent(List<JMM.APEC.EventTracker.Service_EventTrackerReminderList> EventsList)
        {
            var events = from t in EventsList
                         select new ServiceEventTrackerDto()
                         {
                             EventTrackerReminderId=t.EventTrackerReminderId,
                            FacilityName = t.FacilityName,
                            GatewayName = t.GatewayName,
                            CategoryName = t.CategoryName,
                            GatewayId = t.GatewayId,
                            FacilityId = t.FacilityId,
                            CategoryId = t.CategoryId,
                            TypeId = t.TypeId,
                            SubTypeId = t.SubTypeId,
                            TypeName = t.TypeName,
                            SubTypeName = t.SubTypeName,                            
                            DueDate = t.DueDate,
                            DateCompleted = t.DateCompleted,
                            Status = t.Status

                         };

            return events.ToList();
        }

        private List<ServiceEventTrackerCategoryDto> ListEventCategories(List<JMM.APEC.Core.System_Category> Catlist)
        {
            var categorys = from t in Catlist
                            select new ServiceEventTrackerCategoryDto()
                            {
                                CategoryId = t.CategoryId,
                                CategoryName = t.CategoryName
                            };

            return categorys.ToList();
        }

        private List<ServiceEventTrackerTypeDto> ListEventTypes(List<JMM.APEC.Core.System_CategoryType> Typelist)
        {
            var types = from t in Typelist
                        select new ServiceEventTrackerTypeDto()
                        {
                            CategoryId = t.CategoryId,
                            TypeId = t.TypeId,
                            TypeName = t.TypeName
                        };

            return types.ToList();
        }

        private List<ServiceEventTrackerSubTypeDto> ListEventSubTypes(List<JMM.APEC.Core.System_SubType> SubTypelist)
        {
            var Subtypes = from t in SubTypelist
                           select new ServiceEventTrackerSubTypeDto()
                           {
                               TypeId = t.TypeId,
                               SubTypeId = t.SubTypeId,
                               SubTypeName = t.SubTypeName
                           };

            return Subtypes.ToList();
        }



        [Route("reminders")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAllEventReminders([FromUri]EventTrackerBindingModel model)
        {
            List<JMM.APEC.EventTracker.Service_EventTrackerReminderList> EventsList = null;

            if (model != null)
            {

                EventsList = service.GetAllEventTrackerReminders(model.Gateways, model.Facilities, model.Statuses, model.Categories, model.Types, model.Subtypes, model.FromDate, model.ToDate, 1, 25);
            }
            else
            {
                EventsList = service.GetAllEventTrackerReminders(null, null, null, null, null, null, null, null, 0, 0);
            }
                    

            if (EventsList != null)
            {
                var events = ListEvent(EventsList);

                return Ok(new MetadataWrapper<ServiceEventTrackerDto>(events));
            }

            return NotFound();
        }



        [Route("categories")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetEventCategories([FromUri] EventTrackerCategoryBindingModel model)
        {
            List<JMM.APEC.Core.System_Category> catlist = null;
            int ObjectId = 19; //db look up for event tracket object

            if (model != null)
            {
                catlist = service.GetEventCategories(model.Gateways, ObjectId);
            }
            else
            {
                catlist = service.GetEventCategories(null, ObjectId);
            }
            

            if (catlist != null)
            {
                var categories = ListEventCategories(catlist);

                return Ok(new MetadataWrapper<ServiceEventTrackerCategoryDto>(categories));
            }

            return NotFound();
        }


        [Route("types")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetEventTypes([FromUri] EventTrackerTypeBindingModel model)
        {
            List<JMM.APEC.Core.System_CategoryType> typelist = null;
            int ObjectId = 19; //db look up for event tracket object

            if (model != null)
            {
                typelist = service.GetEventTypes(ObjectId, model.Categorys);
            }
            else
            {
                typelist = service.GetEventTypes(ObjectId, null);
            }
           

            if (typelist != null)
            {
                var types = ListEventTypes(typelist);

                return Ok(new MetadataWrapper<ServiceEventTrackerTypeDto>(types));
            }

            return NotFound();
        }

        [Route("subtypes")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetEventSubTypes([FromUri] EventTrackerSubTypeBindingModel model)
        {
            List<JMM.APEC.Core.System_SubType> Subtypelist = null;
            int ObjectId = 19; //db look up for event tracket object

            if (model != null)
            {
                Subtypelist = service.GetEventSubTypes(model.Gateways, ObjectId, model.Types);
            }
            else
            {
                Subtypelist = service.GetEventSubTypes(null, ObjectId, null);
            }
            

            if (Subtypelist != null)
            {
                var subtypes = ListEventSubTypes(Subtypelist);

                return Ok(new MetadataWrapper<ServiceEventTrackerSubTypeDto>(subtypes));
            }

            return NotFound();
        }





    }


}
