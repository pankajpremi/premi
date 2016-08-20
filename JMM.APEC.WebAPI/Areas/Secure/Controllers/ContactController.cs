using JMM.APEC.ActionService;
using JMM.APEC.Core;
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
    [RoutePrefix("api/v1/contacts")]
    [Authorize]
    public class ContactController : SecureApiController
    {
        IService service { get; set; }

        public ContactController()
        {
            service = new Service(CurrentApiUser);
        }



        [Route("{gatewayid:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPost]
        public IHttpActionResult SaveContactDetails(int gatewayid, [FromBody]AssetContactDetailsBindingModel Contact)
        {
            if (gatewayid <= 0)
            {
                return BadRequest();
            }

            if (Contact == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var con = new Asset_ContactInformation();

            con.GatewayId = gatewayid;
            con.ObjectCode = "CONTCT";
            con.ContactId = 0;
            con.TypeId = Contact.TypeId;
            con.TitleId = Contact.TitleId;
            con.FirstName = Contact.FirstName;
            con.LastName = Contact.LastName;
            con.Company = Contact.Company;
            con.Address1 = Contact.Address1;
            con.Address2 = Contact.Address2;
           
            con.City = Contact.City;
            con.CountryCode = Contact.CountryCode;
            con.StateCode = Contact.StateCode;
            con.PostalCode = Contact.PostalCode;
            con.Email = Contact.Email;
            con.IsAutoAdd = Contact.IsAutoAdd;
            con.Facilitylist = Contact.Facilitylist;
            con.Phonelist = Contact.Phonelist;

            var _contact = service.SaveContactDetails(con);

            if(_contact.IsValid())
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }
            else
            {
                return Ok(_contact);
            }




        }


        [Route("{gatewayid:int}/{contactid:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPost]
        public IHttpActionResult UpdateContactDetails(int gatewayid, int contactid, [FromBody]AssetContactDetailsBindingModel Contact)
        {
            if (gatewayid <= 0)
            {
                return BadRequest();
            }

            if (Contact == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var con = new Asset_ContactInformation();

            con.GatewayId = gatewayid;
            con.ObjectCode = "CONTCT";
            con.ContactId = contactid; //input
            con.TypeId = Contact.TypeId;
            con.TitleId = Contact.TitleId;
            con.FirstName = Contact.FirstName;
            con.LastName = Contact.LastName;
            con.Company = Contact.Company;
            con.Address1 = Contact.Address1;
            con.Address2 = Contact.Address2;

            con.City = Contact.City;
            con.CountryCode = Contact.CountryCode;
            con.StateCode = Contact.StateCode;
            con.PostalCode = Contact.PostalCode;
            con.Email = Contact.Email;
            con.IsAutoAdd = Contact.IsAutoAdd;
            con.Facilitylist = Contact.Facilitylist;
            con.Phonelist = Contact.Phonelist;

            var _contact = service.SaveContactDetails(con);

            if (_contact.IsValid())
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }
            else
            {
                return Ok(_contact);
            }




        }




    }

}