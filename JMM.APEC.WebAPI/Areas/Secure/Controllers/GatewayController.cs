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
    [RoutePrefix("api/v1/gateways")]
    public class GatewayController : SecureApiController
    {
        IService service { get; set; }

        public GatewayController()
        {
            service = new Service(CurrentApiUser);
        }

        public GatewayController(IService service) { this.service = service; }

        private List<AssetAddressDto> ListAddress(List<Asset_Address> AddressList)
        {
            var addresses = from t in AddressList
                         select new AssetAddressDto()
                         {
                             AddressId = t.Id,
                             Address1 = t.Address1,
                             Address2 = t.Address2,
                             CornerAddress = t.CornerAddress,
                             City = t.City,
                             PostalCode = t.PostalCode,
                             StateId = t.StateId.GetValueOrDefault(),
                             StateCode = t.State.Code,
                             StateName = t.State.Name,
                             CountryId = t.CountryId.GetValueOrDefault(),
                             CountryCode = t.Country.Code,
                             CountryName = t.Country.Name,
                             TimeZoneId = t.TimeZoneId.GetValueOrDefault(),
                             TimeZoneCode = t.TimeZone.Code,
                             TimeZoneName = t.TimeZone.Name,
                             TimeZoneGMT = t.TimeZone.GMT,
                             TimeZoneOffset = t.TimeZone.Offset,
                             Latitude = t.Latitude.GetValueOrDefault(),
                             Longitude = t.Longitude.GetValueOrDefault(),
                             CountyId = t.CountyId.GetValueOrDefault(),
                             CountyName = t.County.Name
                         };

            return addresses.ToList();
        }

              

        private List<AssetGatewayDto> ListGateways(List<Asset_Gateway> gatewayList)
        {
            var gateways = from t in gatewayList
                           select new AssetGatewayDto()
                           {
                               Id = t.Id,
                               Code = t.Code,
                               Name = t.Name,
                               IdentityId = t.IdentityId,
                               AddressId = t.AddressId.GetValueOrDefault(),
                               StatusId = t.Status.Id,
                               ShortName = t.ShortName,
                               ActiveEndDate = t.EffectiveEndDate.GetValueOrDefault()
                           };

            return gateways.ToList();
        }

        private List<SystemStatusDto> ListStatuses(List<System_Status> statusList)
        {
            var statuses = from t in statusList
                           select new SystemStatusDto()
                           {
                               StatusId = t.Id,
                               StatusCode = t.Code,
                               StatusValue = t.Value,
                               StatusTypeCode = t.StatusType.Code,
                               StatusTypeName = t.StatusType.Name
                           };

            return statuses.ToList();
        }


       private List<AssetFacilityDto> ListFacilities(List<Asset_Facility> facilityList)
        {
            var facilities = from t in facilityList
                             select new AssetFacilityDto()
                             {
                                 GatewayId = t.GatewayId,
                                 FacilityId = t.Id,
                                 FacilityName = t.Name,
                                 FacilityAKA = t.AKAName,
                                 AddressId = t.AddressId.GetValueOrDefault(),
                                 StatusId = t.Status.Id,
                                 StatusCode = t.Status.Code,
                                 StatusValue = t.Status.Value,
                                 StatusDesc = t.Status.Description,
                                 TypeId = t.Type.TypeId,
                                 TypeCode = t.Type.TypeCode,
                                 TypeName = t.Type.TypeName,
                                 PrimaryPhone = t.PrimaryPhoneNumber,
                                 Deleted = t.IsDeleted

                             };

            return facilities.ToList();
        }



        //[Route("{gatewayid:int}/statuses")]
        //[Authorize]
        //[HttpGet]
        //public IHttpActionResult GetStatusList(int? gatewayId)
        //{
        //    List<System_Status> statusList = null;
        //    statusList = service.GetStatusList(gatewayId, null, null);

        //    if (statusList != null)
        //    {
        //        var statuses = ListStatuses(statusList);

        //        return Ok(new MetadataWrapper<SystemStatusDto>(statuses));
        //    }

        //    return NotFound();
        //}

        //[Route("{gatewayid:int}/statuses/{statustypecode}")]
        //[Authorize]
        //[HttpGet]
        //public IHttpActionResult GetStatusList(int? gatewayId, string statusTypeCode)
        //{
        //    List<System_Status> statusList = null;
        //    statusList = service.GetStatusList(gatewayId, statusTypeCode, null);

        //    if (statusList != null)
        //    {
        //        var statuses = ListStatuses(statusList);

        //        return Ok(new MetadataWrapper<SystemStatusDto>(statuses));
        //    }

        //    return NotFound();
        //}

        //[Route("{gatewayid:int}/statuses/{statustypecode}/{statuscode}")]
        //[Authorize]
        //[HttpGet]
        //public IHttpActionResult GetStatusList(int? gatewayId, string statusTypeCode, string statusCode)
        //{
        //    List<System_Status> statusList = null;
        //    statusList = service.GetStatusList(gatewayId, statusTypeCode, statusCode);

        //    if (statusList != null)
        //    {
        //        var statuses = ListStatuses(statusList);

        //        return Ok(new MetadataWrapper<SystemStatusDto>(statuses));
        //    }

        //    return NotFound();
        //}

        [Route("{PortalId:int}/{PortalIsActive:bool}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<AssetGatewayDto> GetGatewayList(int? PortalId, bool? PortalIsActive)
        {
            List<Asset_Gateway> gatewayList = null;
            gatewayList = service.GetGateway(PortalId, PortalIsActive, null, null, null);

            if (gatewayList != null)
            {
                var gateways = ListGateways(gatewayList);

                return gateways.AsEnumerable();
            }

            return null;
        }

        [Route("{PortalId:int}/{PortalIsActive:bool}/{GatewayId:int}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<AssetGatewayDto> GetGatewayList(int? PortalId, bool? PortalIsActive, int? GatewayId)
        {
            List<Asset_Gateway> gatewayList = null;
            gatewayList = service.GetGateway(PortalId, PortalIsActive, GatewayId, null, null);

            if (gatewayList != null)
            {
                var gateways = ListGateways(gatewayList);

                return gateways.AsEnumerable();
            }

            return null;
        }

        [Route("{PortalId:int}/{PortalIsActive:bool}/{GatewayId:int}/{Gatewaycode}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<AssetGatewayDto> GetGatewayList(int? PortalId, bool? PortalIsActive, int? GatewayId, string Gatewaycode)
        {
            List<Asset_Gateway> gatewayList = null;
            gatewayList = service.GetGateway(PortalId, PortalIsActive, GatewayId, Gatewaycode, null);

            if (gatewayList != null)
            {
                var gateways = ListGateways(gatewayList);

                return gateways.AsEnumerable();
            }

            return null;
        }

        [Route("{PortalId:int}/{PortalIsActive:bool}/{GatewayId:int}/{Gatewaycode}/{GatewayStatusId:int}")]
        [Authorize]
        [HttpGet]
        public IEnumerable<AssetGatewayDto> GetGatewayList(int? PortalId, bool? PortalIsActive, int? GatewayId, string Gatewaycode, int GatewayStatusId)
        {
            List<Asset_Gateway> gatewayList = null;
            gatewayList = service.GetGateway(PortalId, PortalIsActive, GatewayId, Gatewaycode, GatewayStatusId);

            if (gatewayList != null)
            {
                var gateways = ListGateways(gatewayList);

                return gateways.AsEnumerable();
            }

            return null;
        }



        [Route(Name = "GetGatewayById")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetGatewayList([FromUri] AssetGatewayBindingModel model)
        {
            List<Asset_Gateway> gatewayList = null;
            if (model != null)
            {
                gatewayList = service.GetGateway(model.PortalId, model.PortalIsActive, model.GatewayId, model.Gatewaycode, model.GatewayStatusCode);
            }

            else
            {
                gatewayList = service.GetGateway(null, null, null, null, null);
            }

            if (gatewayList != null)
            {
                var gateways = ListGateways(gatewayList);

                return Ok(new MetadataWrapper<AssetGatewayDto>(gateways));
            }

            return NotFound();
        }

          
        [Route("{gatewayId:int}/addresses/{addressId:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetGatewayAddress(int gatewayid, int addressid)
        {
            List<Asset_Address> addressList = null;

            if(addressid <= 0)
            {
                return NotFound();
            }
            addressList = service.GetAddress(addressid);

            if (addressList != null)
            {
                var addresses = ListAddress(addressList);

                return Ok(new MetadataWrapper<AssetAddressDto>(addresses));
            }

            return NotFound();
        }

      


        [Route("{gatewayid:int}/facilities")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilities(int gatewayId, [FromUri] AssetFacilityBindingModel model)
        {
            List<Asset_GatewayFacilityList> facilityList = null;
            if (model != null)
            {
                facilityList = service.GetFacilities(gatewayId, model.FacilityId, model.StatusId,model.TypeId, model.GatewayCode, model.GatewayStatusId, model.Searchtext, model.PageNum, model.PageSize, model.SortField, model.SortDirection);
            }
            else
            {
                facilityList = service.GetFacilities(gatewayId, null, null, null, null, null,null, null, null,null,null);
            }

            if(facilityList != null)
            { 

             return Ok(new MetadataWrapper<Asset_GatewayFacilityList>(facilityList));
           }

            return NotFound();
        }

        [Route("{gatewayid:int}/facilities/{facilityid:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilities(int gatewayid, int facilityid)
        {
            List<Asset_Facility> facilityList = null;


            facilityList = service.GetFacility(gatewayid, facilityid, null, null, null, null, null, null, null, null,null);


            if (facilityList != null)
            {
                var facilities = ListFacilities(facilityList);

                return Ok(new MetadataWrapper<AssetFacilityDto>(facilities));
            }

            return NotFound();
        }

        [Route("{gatewayid:int}/contacts")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetContacts(int gatewayId, [FromUri] AssetContactBindingModel model)
        {
            List<Asset_GatewayContactList> ContactList = null;
            if (model != null)
            {
                ContactList = service.GetContact(gatewayId, model.ContactId, "GTEWAY", model.TypeId, model.Active, model.Searchtext, model.PageNum, model.PageSize, model.SortField, model.SortDirection);
            }
            else
            {
                ContactList = service.GetContact(gatewayId, null, "GTEWAY", null, null, null, null, null, null, null);
            }

            if (ContactList != null)
            {
                var contacts = ListContact(ContactList);

                return Ok(new MetadataWrapper<AssetContactDto>(contacts));
            }

            return NotFound();
        }

        private List<AssetContactDto> ListContact(List<Asset_GatewayContactList> ContactList)
        {
            var contacts = from t in ContactList
                           select new AssetContactDto()
                           {
                               ContactId = t.ContactId,
                               ContactType = t.TypeName,
                               ContactTypeId = t.TypeId.GetValueOrDefault(),
                               FirstName = t.FirstName,
                               LastName = t.LastName,
                               Phone = t.Phone,
                               Email = t.Email,
                               Active = t.Active
                           };

            return contacts.ToList();
        }


    }
}
