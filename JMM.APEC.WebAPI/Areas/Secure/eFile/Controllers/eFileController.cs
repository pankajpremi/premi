using JMM.APEC.ActionService;
using JMM.APEC.Efile;
//using JMM.APEC.BusinessObjects;
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
    [RoutePrefix("api/v1/efiles")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,EFILE")]
    public class eFileController : SecureApiController
    {
        IService service { get; set; }

        public eFileController()
        {
            service = new Service(CurrentApiUser);
        }

        public eFileController(IService service) { this.service = service; }

        private List<EFileItemDto> ListeFileItems(List<Service_EfileNode> efileList)
        {
            var tankLevels = from f in efileList
                             select new EFileItemDto()
                             {
                               NodeType = f.Type,
                               ItemId = f.EntityId,
                               ItemName = f.Name,
                               SubNodes = new List<EFileItemDto>()
                             };

            return tankLevels.ToList();
        }


        [Route("services")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult GetEfilesServicesByGateway(EfileBindingModel model)
        {
            List<Service_EfileNode> mediaList = null;

            if (model != null)
            {
                mediaList = service.GetEFileItems(model.Gateways, "Services", model.Keyword);
            }
            else
            {
                mediaList = service.GetEFileItems(null, "Services", null);
            }

            if (mediaList != null)
            {
                //var mediaItems = ListeFileItems(mediaList);

                return Ok(new MetadataWrapper<Service_EfileNode>(mediaList));
            }

            return NotFound();
        }

        [Route("facilities")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult GetEfilesFacilitiesByGateway(EfileBindingModel model)
        {
            List<Service_EfileNode> mediaList = null;

            if (model != null)
            {
                mediaList = service.GetEFileItems(model.Gateways, "Facilities", model.Keyword);
            }
            else
            {
                mediaList = service.GetEFileItems(null, "Facilities", null);
            }

            if (mediaList != null)
            {
                //var mediaItems = ListeFileItems(mediaList);

                return Ok(new MetadataWrapper<Service_EfileNode>(mediaList));
            }

            return NotFound();
        }

    }
}
