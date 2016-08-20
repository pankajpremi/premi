using JMM.APEC.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JMM.APEC.ActionService;
using JMM.APEC.WebAPI.Models;
using JMM.APEC.Core;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/objects")]
    public class ObjectController : SecureApiController
    {
        IService service { get; set; }

        public ObjectController()
        {
            service = new Service(CurrentApiUser);
        }

        private List<SystemObjectDto> ListObjects(List<System_Object> ObjectList)
        {
            var objects = from t in ObjectList
                          select new SystemObjectDto()
                           {
                               ObjectId = t.Id,
                               ObjectCode = t.Code,
                               ObjectName = t.Name,
                               CategoryCode = t.Category.CategoryCode,
                               CategoryName = t.Category.CategoryName
                          };

            return objects.ToList();
        }

        private List<AssetPhoneAssignmentDto> ListPhones(List<Asset_PhoneAssignment> ObjectList)
        {
            var objects = from t in ObjectList
                          select new AssetPhoneAssignmentDto()
                          {
                              
                              PhoneAssignmentID = t.Id,
                              PhoneId = t.Phone.PhoneId,
                              ObjectId = t.Id,
                              EntityId = t.EntityId,
                              TypeId = t.Phone.TypeId,
                              Number = t.Phone.Number

                          };

            return objects.ToList();
        }


        private List<SystemMessageAssignmentDto> ListMessageAssignment(List<System_MessageAssignment> ObjectList)
        {

            List<Asset_GatewayInfo> gtlist = new List<Asset_GatewayInfo>();
            List<SystemMessageAssignmentDto> MessageAssignList = new List<SystemMessageAssignmentDto>();

            SystemMessageAssignmentDto MsgAsn = new SystemMessageAssignmentDto();

            MsgAsn.MessageId = ObjectList[0].MessageId;

            foreach(System_MessageAssignment o in ObjectList)
            {
                Asset_GatewayInfo gt = new Asset_GatewayInfo();
                gt.GatewayId = o.EntityId;

                gtlist.Add(gt);
            }

            MsgAsn.GatewayList = gtlist;

            MessageAssignList.Add(MsgAsn);

            return MessageAssignList;
        }



        [Route("{objectCode}/phones")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetPhoneAssignment(string ObjectCode, [FromUri]SystemObjectBindingModel model)
        {
            List<Asset_PhoneAssignment> PhoneAssignmentList = null;

            if (ObjectCode == null)
            {
                return NotFound();
            }


            if (model != null)
            {
                PhoneAssignmentList = service.GetPhoneList(ObjectCode, model.EntityId, model.Id, model.TypeId);

            }
            else
            {
                PhoneAssignmentList = service.GetPhoneList(ObjectCode, null, null, null);

            }

            if (PhoneAssignmentList != null)
            {
                var phones = ListPhones(PhoneAssignmentList);

                return Ok(new MetadataWrapper<AssetPhoneAssignmentDto>(phones));
            }

            return NotFound();
        }

        //[Route("{objectCode}/contacts")]
        //[Authorize]
        //[HttpGet]
        //public IHttpActionResult GetContactList(string ObjectCode, SystemObjectBindingModel model)
        //{
        //    List<Asset_ObjectContact> objectList = null;

        //    if (ObjectCode == null)
        //    {
        //        return NotFound();
        //    }


        //    if (model != null)
        //    {
        //        objectList = service.GetContactList(ObjectCode, model.objectId, model.EntityId, model.TypeId);

        //    }
        //    else
        //    {
        //        objectList = service.GetContactList(ObjectCode, null, null, null);

        //    }

        //    if (objectList != null)
        //    {
        //        //var objs = ListObjects(objectList);

        //        return Ok(new MetadataWrapper<Asset_ObjectContact>(objectList));
        //    }

        //    return NotFound();
        //}


        [Route("{objectCode}/messages")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetMessageAssignment(string ObjectCode,[FromUri] SystemObjectBindingModel model)
        {
            List<System_MessageAssignment> MessageAssignmentList = null;

            if (ObjectCode == null)
            {
                return NotFound();
            }


            if (model != null)
            {
                MessageAssignmentList = service.GetMessageAssignment(ObjectCode, model.EntityId, model.Id, model.TypeId);

            }
            else
            {
                MessageAssignmentList = service.GetMessageAssignment(ObjectCode, null, null, null);

            }

            if (MessageAssignmentList != null)
            {
                var messages = ListMessageAssignment(MessageAssignmentList);

                return Ok(new MetadataWrapper<SystemMessageAssignmentDto>(messages));
            }

            return NotFound();
        }



        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPost]
        public HttpResponseMessage SaveObjectList(CreateObjectBindingModel[] objects)
        {
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            List<System_Object> objectList = new List<System_Object>();
            foreach (var obj in objects)
            {
                var o = new System_Object();
                o.Id = obj.ObjectId; //0 for insert
                o.CategoryId = obj.CategoryId;
                o.Code = obj.ObjectCode;
                o.Name = obj.ObjectName;

                objectList.Add(o);
            }

            service.SaveObjectItem(objectList);

            response = Request.CreateResponse(HttpStatusCode.OK, objects);
            return response;
        }

        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpDelete]
        public HttpResponseMessage DeleteObjectList(CreateObjectBindingModel[] objects)
        {
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            List<System_Object> objectList = new List<System_Object>();
            foreach (var obj in objects)
            {
                var o = new System_Object();
                o.Id = obj.ObjectId; 
                o.CategoryId = obj.CategoryId;
                o.Code = obj.ObjectCode;
                o.Name = obj.ObjectName;

                objectList.Add(o);
            }


            service.DeleteObjectItem(objectList);

            response = Request.CreateResponse(HttpStatusCode.OK, objects);
            return response;
        }




    }

}
