using JMM.APEC.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JMM.APEC.ActionService;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.WebAPI.Models;
using JMM.APEC.Core;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/categories")]
    [Authorize]
    public class CategoryController : SecureApiController
    {
        IService service { get; set; }

        public CategoryController()
        {
            service = new Service(CurrentApiUser);
        }

        private List<SystemCategoryDto> ListCategories(List<System_Category> categoryList)
        {
            var cats = from t in categoryList
                          select new SystemCategoryDto()
                           {
                               CategoryId = t.CategoryId,
                               CategoryName = t.CategoryName,
                               CategoryCode = t.CategoryCode,
                               Active = t.Active ,
                               ObjectCode = t.Object.Code,
                               ObjectName = t.Object.Name
                           };

            return cats.ToList();
        }



        [Route("{gatewayid:int}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<SystemCategoryDto> GetCategoryList(int? gatewayId)
        {
            List<System_Category> catList = null;
            catList = service.GetCategoryList(gatewayId, null, null);

            if (catList != null)
            {
                var categories = ListCategories(catList);

                return categories.AsEnumerable();
            }

            return null;
        }

        [Route("{gatewayid:int}/{objectId:int}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<SystemCategoryDto> GetCategoryList(int? gatewayId, int? ObjectId)
        {
            List<System_Category> catList = null;
            catList = service.GetCategoryList(gatewayId, ObjectId, null);

            if (catList != null)
            {
                var categories = ListCategories(catList);

                return categories.AsEnumerable();
            }

            return null;
        }

        [Route("{gatewayid:int}/{objectId:int}/{Active:bool}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<SystemCategoryDto> GetCategoryList(int? gatewayId, int? ObjectId, bool? active)
        {
            List<System_Category> catList = null;
            catList = service.GetCategoryList(gatewayId, ObjectId, active);

            if (catList != null)
            {
                var categories = ListCategories(catList);

                return categories.AsEnumerable();
            }

            return null;
        }

        [Route("{gatewayid:int}/{Active:bool}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<SystemCategoryDto> GetCategoryList(int? gatewayId, bool? active)
        {
            List<System_Category> catList = null;
            catList = service.GetCategoryList(gatewayId, null, active);

            if (catList != null)
            {
                var categories = ListCategories(catList);

                return categories.AsEnumerable();
            }

            return null;
        }


        //[Route("")]
        //[Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        //[HttpPost]
        //public HttpResponseMessage SaveCategoryList(CreateCategoryBindingModel[] categories)
        //{
        //    HttpResponseMessage response = null;

        //    if (!ModelState.IsValid)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest);
        //        return response;
        //    }

        //    List<System_Category> catList = new List<System_Category>();
        //    foreach (var cat in categories)
        //    {
        //        var s = new System_Category();
        //        s.CategoryId = cat.Id; //0 for insert
        //        s.ObjectId = cat.ObjectId;
        //        s.GatewayId = cat.GatewayId;
        //        s.CategoryCode = cat.CategoryCode;
        //        s.CategoryName = cat.CategoryName;
        //        s.Active = true;

        //        catList.Add(s);
        //    }

        //    service.SaveCategoryItem(catList);

        //    response = Request.CreateResponse(HttpStatusCode.OK, categories);
        //    return response;
        //}

        //[Route("")]
        //[Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        //[HttpDelete]
        //public HttpResponseMessage DeleteCategoryList(CreateCategoryBindingModel[] categories)
        //{
        //    HttpResponseMessage response = null;

        //    if (!ModelState.IsValid)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest);
        //        return response;
        //    }

        //    List<System_Category> catList = new List<System_Category>();
        //    foreach (var cat in categories)
        //    {
        //        var s = new System_Category();
        //        s.CategoryId = cat.Id; 
        //        s.ObjectId = cat.ObjectId;
        //        s.GatewayId = cat.GatewayId;
        //        s.CategoryCode = cat.CategoryCode;
        //        s.CategoryName = cat.CategoryName;
        //        s.Active = true;

        //        catList.Add(s);
        //    }

        //    service.DeleteCategoryItem(catList);

        //    response = Request.CreateResponse(HttpStatusCode.OK, categories);
        //    return response;
        //}

    }
}
