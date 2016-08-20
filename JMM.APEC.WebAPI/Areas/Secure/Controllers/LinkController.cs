using JMM.APEC.ActionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using JMM.APEC.Core;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/links")]
    public class LinkController : SecureApiController
    {
        IService service { get; set; }
       
        public LinkController()
        {
            service = new Service(CurrentApiUser);
        }

        private List<System_UserLinkDto> ListFavLinks(List<System_UserLink> linkList)
        {
            var links = from t in linkList
                        select new System_UserLinkDto()
                             {
                                 LinkName = t.LinkName,
                                 LinkUrl = t.LinkUrl
                             };

            return links.ToList();
        }
        
        [Route("favorites", Name = "DefaultApi")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFavLinksList()
        {
            //List<System_UserLink> LinksList = null;
            //int UserId = 3;
            //LinksList = service.GetUserFavLinkList();

            //if (LinksList != null)
            //{
            //    var links = ListFavLinks(LinksList);

            //    return links.AsEnumerable();
            //}

            List<System_UserLink> linklist = new List<System_UserLink>();

            System_UserLink link1 = new System_UserLink();
            System_UserLink link2 = new System_UserLink();
            System_UserLink link3 = new System_UserLink();
           

            link1.LinkName = "Alarm Dispatch";
            link1.LinkUrl = "http://tankcomply.com/5_2_2_service_alarms.html";

            link2.LinkName = "Event Tracker";
            link2.LinkUrl = "http://tankcomply.com/5_6_services_event_tracker.html";

            link3.LinkName = "Tank Levels";
            link3.LinkUrl = "http://tankcomply.com/5_4_service_fuel_mgmnt.html";

            linklist.Add(link1);
            linklist.Add(link2);
            linklist.Add(link3);

            return Ok(new MetadataWrapper<System_UserLink>(linklist));
        
    }
    }
}
