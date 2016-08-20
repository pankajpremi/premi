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
    [RoutePrefix("api/v1/types")]
    public class TypeController : SecureApiController
    {

        IService service { get; set; }

        public TypeController()
        {
            service = new Service(CurrentApiUser);
        }

        private List<SystemTypeDto> ListTypes(List<System_Type> typeList)
        {
            var types = from t in typeList
                        select new SystemTypeDto()
                        {
                            ObjectId = t.ObjectId,
                            GatewayId = t.GatewayId,
                            TypeCode = t.TypeCode,
                            TypeName = t.TypeName,
                            TypeId = t.TypeId
                           };

            return types.ToList();
        }


        [Route()]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetTypeList([FromUri] SystemTypeBindingModel model)
        {
            List<System_Type> typeList = null;

            if(model != null)
            {
                typeList = service.GetTypeList(model.ObjectId, model.ObjectCode, model.GatewayIds, model.TypeCode);
            }
            else
            {
                typeList = service.GetTypeList(null, null, null, null);
            }
            
            if (typeList != null)
            {
                var Types = ListTypes(typeList);

                return Ok(new MetadataWrapper<SystemTypeDto>(Types));

                
            }

            return NotFound();
        }


        //[Route("")]
        //[Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        //[HttpPost]
        //public HttpResponseMessage SaveTypeList(CreateTypeBindingModel[] types)
        //{
        //    HttpResponseMessage response = null;

        //    if (!ModelState.IsValid)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest);
        //        return response;
        //    }

        //    List<System_Type> typeList = new List<System_Type>();
        //    foreach (var type in types)
        //    {
        //        var t = new System_Type();
        //        t.TypeId = type.TypeId; //0 for insert
        //        t.ObjectId = type.ObjectId;
        //        t.GatewayId = type.GatewayId;
        //        t.TypeCode = type.TypeCode;
        //        t.TypeName = type.TypeName;
        //        t.Active = type.IsActive;

        //        typeList.Add(t);
        //    }

        //    service.SaveTypeItem(typeList);

        //    response = Request.CreateResponse(HttpStatusCode.OK, types);
        //    return response;
        //}


        //[Route("")]
        //[Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        //[HttpDelete]
        //public HttpResponseMessage DeleteTypeList(CreateTypeBindingModel[] types)
        //{
        //    HttpResponseMessage response = null;

        //    if (!ModelState.IsValid)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest);
        //        return response;
        //    }

        //    List<System_Type> typeList = new List<System_Type>();
        //    foreach (var type in types)
        //    {
        //        var t = new System_Type();
        //        t.TypeId = type.TypeId; //0 for insert
        //        t.ObjectId = type.ObjectId;
        //        t.GatewayId = type.GatewayId;
        //        t.TypeCode = type.TypeCode;
        //        t.TypeName = type.TypeName;
        //        t.Active = type.IsActive;

        //        typeList.Add(t);
        //    }


        //    service.DeleteTypeItem(typeList);

        //    response = Request.CreateResponse(HttpStatusCode.OK, types);
        //    return response;
        //}




    }
}
