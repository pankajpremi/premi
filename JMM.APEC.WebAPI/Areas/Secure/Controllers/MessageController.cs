using JMM.APEC.ActionService;
using JMM.APEC.Core;
using JMM.APEC.WebAPI.Areas.Admin.Models;
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
    [RoutePrefix("api/v1/messages")]
    [Authorize]
    public class MessageController :  SecureApiController
    {

        IService service { get; set; }
        IIdentityService identService { get; set; }

        public MessageController()
        {
            service = new Service(CurrentApiUser);
            identService = new IdentityService(CurrentApiUser);
        }
        private List<SystemMessageDto> ListMessage(List<System_MessageList> MsgList)
        {
            var msgs = from t in MsgList
                      select new SystemMessageDto()
                      {
                          MessageId = t.MessageId,
                          Subject = t.Subject,
                          Message = t.Message,
                          BeginDateTime = t.BeginDateTime,
                          EndDateTime = t.EndDateTime,
                          IsDismissible = t.IsDismissible,
                          Status = t.Status,
                          GatewayCount = t.GatewayCount,
                          IsDeleted = t.IsDeleted

                      };

            return msgs.ToList();
        }


        [Route("{objectcode}/{typecode}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetMessageAssignmentList(string objectcode, string typecode,[FromUri]SystemMessageBindingModel model)
        {
            List<System_MessageList> MsgList = null;

            if (model != null)
            {
                MsgList =  service.GetMessage(model.MessageId, objectcode, typecode, model.UserToId, model.UserFromId, model.Searchtext, model.PageNum, model.PageSize, model.SortField, model.SortDirection);
            }
            else
            {
                MsgList =service.GetMessage(null, objectcode, typecode, null, null, null, null, null, null, null);
            }

            if (MsgList != null)
            {
               var Messages =  ListMessage(MsgList);
              return Ok(new MetadataWrapper<SystemMessageDto>(Messages));
            }

            return NotFound();
        }

        
        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPost]
        public IHttpActionResult SaveMessageAssignmnent([FromBody]SystemMessageGatewayBindingModel msg)
        {
            HttpResponseMessage response = null;

            if(msg == null)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<System_MessageAssignmentList> msgatlist = new List<System_MessageAssignmentList>();

            var msgat = new System_MessageAssignmentList();
         
            msgat.MessageId = 0;
            msgat.TypeCode = msg.TypeCode;
            msgat.ObjectCode = msg.ObjectCode;
            msgat.Subject = msg.Subject;
            msgat.Message = msg.Message;
            msgat.BeginDateTime = msg.BeginDateTime;
            msgat.EndDateTime = msg.EndDateTime.GetValueOrDefault();
            msgat.IsDismissible = msg.IsDismissible;
            //msgat.UserFromId = msg.FromUserId;
            msgat.gatewaylist = msg.gatewaylist;

             var msgasg = service.InsertMessageAssignment(msgat);

            if (msgasg.IsValid())
            {
                return Ok(msgasg);
            }
            else
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }


        }


        [Route("{messageid:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPut]
        public IHttpActionResult UpdateMessageAssignment(int MessageId, [FromBody] SystemMessageGatewayBindingModel msg)
        {
             if(MessageId <= 0)
            {

                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<System_MessageAssignmentList> msgatlist = new List<System_MessageAssignmentList>();

            var msgat = new System_MessageAssignmentList();
            msgat.TypeCode = msg.TypeCode;
            msgat.ObjectCode = msg.ObjectCode;
            msgat.Subject = msg.Subject;
            //msgat.UserFromId = msg.FromUserId;
            msgat.MessageId = MessageId; //input
            msgat.Message = msg.Message;
            msgat.BeginDateTime = msg.BeginDateTime;
            msgat.EndDateTime = msg.EndDateTime.GetValueOrDefault();
            msgat.IsDismissible = msg.IsDismissible;
            msgat.gatewaylist = msg.gatewaylist;

            var msgasg = service.UpdateMessageAssignment(msgat);

            if (msgasg.IsValid())
            {
                return Ok(msgasg);
            }
            else
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }

                     
        }

        [Route("{MessageId:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpDelete]
        public HttpResponseMessage DeleteMessage(int MessageId)
        {
            HttpResponseMessage response = null;

            if (MessageId <= 0)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, Resources.LangResource.BadInput);
            return response;
            }

                    
            if (service.DeleteMessage(MessageId) < 0)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, Resources.LangResource.DBErrorMessage);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
            }

            return response;
        }


        [Route("users/{userid:int}")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult SendPrivateMessageToUser(int userid, SendMessageBindingModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userid <= 0)
            {
                return BadRequest();
            }

            if (message == null)
            {
                return BadRequest();
            }

            var user = identService.GetUser(userid);
            if (user == null)
            {
                return NotFound();
            }

            var UserMessage = service.SendUserMessage(userid, message.Subject, message.Message);

            if (UserMessage.IsValid())
            {
                return Ok(Code.MessageHelper.MakeMessageDto(UserMessage));
            }
            else
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }
        }

    }
}
