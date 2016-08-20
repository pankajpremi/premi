using JMM.APEC.ActionService;
using JMM.APEC.Core;
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
    [RoutePrefix("api/v1/activitylogs")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,ACTLOG")]
    public class ActivityLogController : SecureApiController
    {
        IService service { get; set; }

        public ActivityLogController()
        {
            service = new Service(CurrentApiUser);
        }
        public ActivityLogController(IService service)
        {
            this.service = service;
        }

        private List<ServiceActivityLogDto> ListActivityLog(List<JMM.APEC.ActivityLog.Service_ActivityLog> ActivityLogList)
        {
            var logs = from t in ActivityLogList
                             select new ServiceActivityLogDto()
                             {
                                 ActivityLogId = t.ActivityLogId,
                                 FacilityName = t.FacilityName,
                                 Body = t.Body,
                                 EnteredBy = t.EnteredBy,
                                 Title = t.Title,
                                 UserId = t.UserId,
                                 //TypeId = t.Type.TypeId,
                                 //TypeName = t.Type.TypeName,
                                 LogDateTime = t.LogDate,
                                 GatewayId = t.GatewayId,
                                 NumOfComments = t.Totalcomments
                             };

            return logs.ToList();
        }

        private List<ServiceActivityLogTypeDto> ListALTypes(List<System_Type> ActivityLogTypeList)
        {
            var types = from t in ActivityLogTypeList
                       select new ServiceActivityLogTypeDto()
                       {
                           TypeId = t.TypeId,
                           TypeCode = t.TypeCode,
                           TypeName = t.TypeName,
                           GatewayId = t.GatewayId
                       };

            return types.ToList();
        }

        private List<ServiceActivityLogCommentDto> ListActivityLogComment(List<JMM.APEC.ActivityLog.Service_ActivityLogComment> ActivityLogCommentList)
        {
            var comments = from t in ActivityLogCommentList
                       select new ServiceActivityLogCommentDto()
                       {
                           ActivityLogId = t.ActivityLogId,
                          CommentId = t.CommentId,
                          Comment = t.Comment,
                          CommentDateTime = t.CommentDateTime,
                          EnteredBy = t.EnteredBy
                       };

            return comments.ToList();
        }


        private List<ServiceActivityLogMediaDto> ListActivityLogMedia(List<JMM.APEC.ActivityLog.Service_ActivityLogMedia> ActivityLogMediaList)
        {
            var medias = from t in ActivityLogMediaList
                           select new ServiceActivityLogMediaDto()
                           {
                               ActivityLogId = t.ActivityLogId,
                               MediaId = t.MediaId,
                               FileName = t.FileName
                           };

            return medias.ToList();
        }


        [Route("")]
        [Authorize]
        [HttpGet]
        //Pulls list of logs - 10/27/2015
        public IHttpActionResult GetAllActivityLogs([FromUri] ServiceActivityLogBindingModel model)
        {
            List<JMM.APEC.ActivityLog.Service_ActivityLog> ALList = null;

            if (model != null)
            {

                ALList = service.GetAllActivityLogs(model.Gateways, model.Facilities, model.AlTypes, model.FromDate, model.ToDate);
            }
            else
            {
                ALList = service.GetAllActivityLogs(null, null, null, null, null);
            }
                       

            if (ALList != null)
            {
                var logs = ListActivityLog(ALList);

                return Ok(new MetadataWrapper<ServiceActivityLogDto>(logs));
            }

            return NotFound();
        }


        [Route("{activitylogid:int}/comments")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetActivityLogComments(int activitylogid)
        {
            List<JMM.APEC.ActivityLog.Service_ActivityLogComment> ALCommentsList = null;
            ALCommentsList = service.GetActivityLogComments(activitylogid);

            if (ALCommentsList != null)
            {
                var comments = ListActivityLogComment(ALCommentsList);

                return Ok(new MetadataWrapper<ServiceActivityLogCommentDto>(comments));
            }

            return NotFound();
        }


        [Route("{activitylogid:int}/medias")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetActivityLogMedias(int activitylogid)
        {
            List<JMM.APEC.ActivityLog.Service_ActivityLogMedia> ALMediasList = null;
            ALMediasList = service.GetActivityLogMedia(activitylogid);

            if (ALMediasList != null)
            {
                var medias = ListActivityLogMedia(ALMediasList);

                return Ok(new MetadataWrapper<ServiceActivityLogMediaDto>(medias));
            }

            return NotFound();
        }

        [Route("types")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetActivityLogTypes([FromUri] ServiceActivityLogTypeBindingModel model)
        {
            List<System_Type> typelist = null;
            int ObjectId = 12; //db look up for AL object

            if (model != null)
            {
                typelist = service.GetActivityLogTypes(ObjectId, model.GatewayIds,model.TypeCode);
            }
            else
            {
                typelist = service.GetActivityLogTypes(ObjectId, null, null);
            }


            if (typelist != null)
            {
                var types = ListALTypes(typelist);

                return Ok(new MetadataWrapper<ServiceActivityLogTypeDto>(types));
            }

            return NotFound();
        }




    }
}
