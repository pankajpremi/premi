using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JMM.APEC.ActionService;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using JMM.APEC.WebAPI.Areas.Secure.Models;
using JMM.APEC.Core;
using JMM.APEC.IMS;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/facilities")]
    [Authorize]
    public class FacilityController : SecureApiController
    {
        IService service { get; set; }

        public FacilityController()
        {
            service = new Service(CurrentApiUser);
        }

        private List<AssetFacilityDto> ListFacilities(List<Asset_Facility> facilityList)
        {
            var facilities = from t in facilityList
                             select new AssetFacilityDto()
                           {
                               FacilityId = t.Id,
                               FacilityName = t.Name,
                               FacilityAKA = t.AKAName,
                               AddressId = t.AddressId.GetValueOrDefault(),
                               StatusId = t.Status.Id,
                               StatusCode = t.Status.Code,
                               StatusValue = t.Status.Value,
                               StatusDesc =t.Status.Description,
                               TypeId = t.Type.TypeId,
                               TypeCode = t.Type.TypeCode,
                               TypeName = t.Type.TypeName,
                               PrimaryPhone = t.PrimaryPhoneNumber,
                               Deleted = t.IsDeleted,
                               StateId = t.Address.State.Id,
                               StateCode = t.Address.State.Code,
                               StateName = t.Address.State.Name,
                               CountryId = t.Address.Country.Id,
                               CountryCode = t.Address.Country.Code,
                               CountryName = t.Address.Country.Name,
                               CountyId = t.Address.County.Id,
                               CountyCode = t.Address.County.Code,
                               CountyName = t.Address.County.Name

                             };

            return facilities.ToList();
        }

        private List<AssetFacilityMapDto> ListFacilityMap(List<Asset_Facility> facilityList)
        {
            var facilityMap = from t in facilityList
                             select new AssetFacilityMapDto()
                             {
                                 FacilityId = t.Id,
                                 FacilityName = t.Name,
                                 Address1 = t.Address.Address1,
                                 Address2 = t.Address.Address2,
                                 City = t.Address.City,
                                 Zip = t.Address.PostalCode,
                                 State = t.Address.State.Name,
                                 Country = t.Address.Country.Name,
                                 Latitude = t.Address.Latitude.GetValueOrDefault(),
                                 Longitude = t.Address.Longitude.GetValueOrDefault()
                             };

            return facilityMap.ToList();
        }

        private List<ServiceActivityLogDto> ListActivityLog(List<JMM.APEC.ActivityLog.Service_ActivityLog> ALList)
        {
            var allogs = from t in ALList
                             select new ServiceActivityLogDto()
                             {
                                 ActivityLogId = t.ActivityLogId,
                                 Title = t.Title,
                                 UserId = t.UserId,
                                 //TypeName = t.Type.TypeName,
                                 LogDateTime = t.LogDate,
                                 GatewayId = t.GatewayId
                             };

            return allogs.ToList();
        }

        //private List<AssetFacilityContactPhoneDto> ListFacilityContacts(List<Asset_FacilityContactPhone> facilityContactList)
        //{
        //    var facilitycontacts = from t in facilityContactList
        //                           select new AssetFacilityContactPhoneDto()
        //                           {
        //                                FacilityId = t.FacilityContact.Facility.Id,
        //                                FacilityName = t.FacilityContact.Facility.Name,
        //                                ContactId = t.FacilityContact.Contact.ContactId,
        //                                ContactName = t.FacilityContact.Contact.FirstName + " " + t.FacilityContact.Contact.LastName,
        //                                ContactCompany = t.FacilityContact.Contact.Company,
        //                                ContactTitle = t.FacilityContact.Contact.Title,
        //                                PhoneId = t.PhoneId,
        //                                PhoneNumber = t.Phone.Number,
        //                                PhoneTypeId = t.Phone.TypeId,
        //                                PhoneType = t.Phone.Type.TypeName
        //                           };                                                                  

        //    return facilitycontacts.ToList();
        //}

        private List<ServiceEventTrackerDto> ListEvent(List<JMM.APEC.EventTracker.Service_EventTrackerReminder> EventsList)
        {
            var events = from t in EventsList
                         select new ServiceEventTrackerDto()
                         {
                             CategoryId = t.CategoryId,
                             TypeId = t.TypeId,
                             SubTypeId = t.SubTypeId,
                             GatewayId = t.GatewayId,
                             DueDate = DateTime.Now.Date
                            // ParentId = t.ParentId

                                   };

            return events.ToList();
        }

        private List<TankLevelDto> ListTankLevelsWithLastDelivery(List<Service_EventCompartmentLevel> TankLevelList)
        {
            var events = from t in TankLevelList
                         select new TankLevelDto()
                         {
                             AtgId = t.AtgId,
                             CapacityAmt = t.CapacityAmt,
                             EstEmpty = t.ReadingDateTime.Value.AddDays(5),
                             ReadingDateTime = t.ReadingDateTime,
                             FacilityId = t.FacilityId,
                             GatewayId = t.GatewayId,
                             HeightAmt = t.HeightAmt,
                             PercentVolumeAmt = t.PercentVolumeAmt,
                             ReadingDateInt = t.ReadingDateInt,
                             ReadingTimeInt = t.ReadingTimeInt,
                             TankCompartmentId = t.TankCompartmentId,
                             TankCompartmentLabel = t.TankCompartmentLabel,
                             TankCompartmentNumber = t.TankCompartmentNumber,
                             ProductLabel = t.ProductLabel,
                             UllageAmt = t.UllageAmt,
                             VolumeAmt = t.VolumeAmt,
                             WaterHeightAmt = t.WaterHeightAmt,
                             LatestDelivery = new TankDeliveryDto
                             {
                                 AtgId = t.AtgId,
                                 DeliveredDateTime = t.LatestDelivery.DeliveredDateTime,
                                 DeliveredMeasAmt = t.LatestDelivery.DeliveredMeasAmt,
                                 DeliveredVolAmt = t.LatestDelivery.DeliveredVolAmt,
                                 EventId = t.LatestDelivery.EventId,
                                 FacilityId = t.FacilityId,
                                 FacilityName = t.FacilityName,
                                 GatewayId = t.GatewayId,
                                 GatewayName = t.GatewayName,
                                 ProductLabel = t.ProductLabel,
                                 TankCompartmentId = t.TankCompartmentId,
                                 TankCompartmentLabel = t.TankCompartmentLabel,
                                 TankCompartmentNumber = t.TankCompartmentNumber
                             }
                         };

            return events.ToList();
        }

        private List<FlowMeterDto> ListFlowMeters(List<Service_FlowMeter> FlowMeterList)
        {
            var events = from t in FlowMeterList
                         select new FlowMeterDto()
                         {
                             EventId = t.EventId,
                             AvgGPMValue = t.AvgGPMValue,
                             FlowMeterNumber = t.FlowMeterNumber,
                             FuelPositionId = t.FuelPositionId,
                             FuelPositionLabel = t.FuelPositionLabel,
                             FuelPositionNumber = t.FuelPositionNumber,
                             HoseNumber = t.HoseNumber,
                             NumOfTransactions = t.NumOfTransactions,
                             ReadingDateTime = t.ReadingDateTime,
                             TotalVolumeAmt = t.TotalVolumeAmt
                         };

            return events.ToList();
        }



        private List<AssetAddressDto> ListAddress(List<Asset_Address> AddressList)
        {
            var events = from t in AddressList
                         select new AssetAddressDto()
                         {
                            AddressId = t.Id,
                            Address1 = t.Address1,
                            Address2 = t.Address2,
                            CornerAddress = t.CornerAddress,
                            City =t.City,
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
                            Latitude =t.Latitude.GetValueOrDefault(),
                            Longitude = t.Longitude.GetValueOrDefault()
                         };

            return events.ToList();
        }


        //[Route("{GatewayId:int}")]
        //[Authorize]
        //[HttpGet]
        //public IEnumerable<AssetFacilityDto> GetFacilityList(int GatewayId)
        //{
        //    List<Asset_Facility> facilityList = null;
        //    facilityList = service.GetFacility(GatewayId, null, null, null, null);

        //    if (facilityList != null)
        //    {
        //        var facilities = ListFacilities(facilityList);

        //        return facilities.AsEnumerable();
        //    }

        //    return null;
        //}

        //[Route("{GatewayId:int}/{FacilityId:int}")]
        //[Authorize]
        //[HttpGet]
        //public IEnumerable<AssetFacilityDto> GetFacilityList(int GatewayId, int FacilityId)
        //{
        //    List<Asset_Facility> facilityList = null;
        //    facilityList = service.GetFacility(GatewayId, FacilityId, null, null, null);

        //    if (facilityList != null)
        //    {
        //        var facilities = ListFacilities(facilityList);

        //        return facilities.AsEnumerable();
        //    }

        //    return null;
        //}

        //[Route("{gatewayid:int}/{facilityId:int}/{statusId:int}")]
        //[Authorize]
        //[HttpGet]
        //public IEnumerable<AssetFacilityDto> GetFacilityList(int GatewayId, int FacilityId, int StatusId)
        //{
        //    List<Asset_Facility> facilityList = null;
        //    facilityList = service.GetFacility(GatewayId, FacilityId, StatusId, null, null);

        //    if (facilityList != null)
        //    {
        //        var facilities = ListFacilities(facilityList);

        //        return facilities.AsEnumerable();
        //    }

        //    return null;
        //}

        //[Route("{gatewayid:int}/{facilityId:int}/{statusId:int}/{gatewaycode}")]
        //[Authorize]
        //[HttpGet]
        //public IEnumerable<AssetFacilityDto> GetFacilityList(int GatewayId, int FacilityId, int StatusId, string GatewayCode)
        //{
        //    List<Asset_Facility> facilityList = null;
        //    facilityList = service.GetFacility(GatewayId, FacilityId, StatusId, GatewayCode, null);

        //    if (facilityList != null)
        //    {
        //        var facilities = ListFacilities(facilityList);

        //        return facilities.AsEnumerable();
        //    }

        //    return null;
        //}

        //[Route("{gatewayid:int}/{facilityId:int}/{statusId:int}/{gatewayCode}/{gatewaystatuscode:int}")]
        //[Authorize]
        //[HttpGet]
        //public IEnumerable<AssetFacilityDto> GetFacilityList(int GatewayId, int FacilityId, int StatusId, string GatewayCode, int GatewayStatusCode)
        //{
        //    List<Asset_Facility> facilityList = null;
        //    facilityList = service.GetFacility(GatewayId, FacilityId, StatusId, GatewayCode, GatewayStatusCode);

        //    if (facilityList != null)
        //    {
        //        var facilities = ListFacilities(facilityList);

        //        return facilities.AsEnumerable();
        //    }

        //    return null;
        //}



        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPost]
        public HttpResponseMessage InsertFacility(CreateFacilityBindingModel[] facility)
        {
            HttpResponseMessage response = null;

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            }

            List<Asset_Facility> facilities = new List<Asset_Facility>();

            foreach (CreateFacilityBindingModel f in facility)
            {
                var fac = new Asset_Facility();

                fac.Id = 0;
                fac.GatewayId = f.GatewayId;
                fac.AKAName = f.AKAName;
                fac.Name = f.FacilityName;
                fac.TypeId = f.TypeId;
                fac.StatusId = f.StatusId;
                fac.AddressId = f.AddressId;
                fac.IsDeleted = f.IsDeleted;
                fac.AppChangeUserId = f.AppChangeUserId;

                facilities.Add(fac);
            }

            if(service.InsertFacility(facilities) < 0)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, Resources.LangResource.DBErrorMessage);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            return response;
        }

        [Route("")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPut]
        public HttpResponseMessage UpdateFacility(int FacilityId,CreateFacilityBindingModel[] facility)
        {
            HttpResponseMessage response = null;

            if (FacilityId <= 0)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, Resources.LangResource.BadInput);
                return response;
            }

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                return response;
            }

            List<Asset_Facility> facilities = new List<Asset_Facility>();

            foreach (CreateFacilityBindingModel f in facility)
            {
                var fac = new Asset_Facility();

                fac.Id = FacilityId;
                fac.GatewayId = f.GatewayId;
                fac.AKAName = f.AKAName;
                fac.Name = f.FacilityName;
                fac.TypeId = f.TypeId;
                fac.StatusId = f.StatusId;
                fac.AddressId = f.AddressId;
                fac.IsDeleted = f.IsDeleted;
                fac.AppChangeUserId = f.AppChangeUserId;

                facilities.Add(fac);
            }


            if (service.UpdateFacility(facilities) < 0)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, Resources.LangResource.DBErrorMessage);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            return response;
        }

        [Route("{FacilityId:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpDelete]
        public HttpResponseMessage DeleteFacility(int FacilityId)
        {
            HttpResponseMessage response = null;

            if(FacilityId <= 0)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, Resources.LangResource.BadInput);
                return response;
            }


            if(service.DeleteFacility(FacilityId) < 0)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, Resources.LangResource.DBErrorMessage);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
            }

            return response;
        }


        [Route("top")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetTopFacilityList()
        {
            List<Asset_Facility> facilityList = null;
            facilityList = service.GetTopFacilities();

            if (facilityList != null)
            {
                var facilities = ListFacilities(facilityList);

                 return Ok(new MetadataWrapper<AssetFacilityDto>(facilities)); 
            }

            return NotFound(); 
        }



        [Route("{facilityid:int}/contacts")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetContactsByFacility(int facilityId)
        {
            List<Asset_Contact> facilityContactList = null;
            facilityContactList = service.GetFacilityContactsByFacilityId(facilityId);

            if (facilityContactList != null)
            {
               // var facilityContacts = ListFacilityContacts(facilityContactList);

                return Ok(new MetadataWrapper<Asset_Contact>(facilityContactList));
            }

            return NotFound();
        }

        [Route("{facilityid:int}/maps")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilityMaps(int facilityId)
        {
            List<Asset_Facility> facilityList = null;
           
            //facilityList = service.GetFacilityLatLong(facilityId);

            if (facilityList != null)
            {
                var facilities = ListFacilityMap(facilityList);

                return Ok(new MetadataWrapper<AssetFacilityMapDto>(facilities));
            }

            return NotFound();
        }

        [Route("{facilityid:int}/activitylogs")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetActivityLogsByFacility(int FacilityId)
        {
            List<JMM.APEC.ActivityLog.Service_ActivityLog> ALList = null;
            ALList = service.GetActivityLogsByFacility(FacilityId);

            if (ALList != null)
            {
                var logs = ListActivityLog(ALList);

                return Ok(new MetadataWrapper<ServiceActivityLogDto>(logs));
            }

            return NotFound();
        }


        [Route("{facilityid:int}/events")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetEventRemindersByFacility(int FacilityId)
        {
            List<JMM.APEC.EventTracker.Service_EventTrackerReminder> EventsList = null;
            EventsList = service.GetEventTrackerRemindersByFacility(FacilityId);

            if (EventsList != null)
            {
                var events = ListEvent(EventsList);

                return Ok(new MetadataWrapper<ServiceEventTrackerDto>(events));
            }

            return NotFound();
        }

        [Route("{facilityid:int}/tanklevels")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilityLatestTankLevels(int facilityId)
        {
            List<Service_EventCompartmentLevel> TankLevelList = null;

            TankLevelList = service.GetLatestCompartmentTankLevelsForFacility(facilityId);

            if (TankLevelList != null)
            {
                var levels = ListTankLevelsWithLastDelivery(TankLevelList);

                return Ok(new MetadataWrapper<TankLevelDto>(levels));
            }

            return NotFound();
        }

        [Route("{facilityid:int}/flowmeters")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilityLatestFlowMeters(int facilityId)
        {
            List<Service_FlowMeter> FlowMeterList = null;

            FlowMeterList = service.GetLatestFlowMetersForFacility(facilityId);

            if (FlowMeterList != null)
            {
                var meters = ListFlowMeters(FlowMeterList);

                return Ok(new MetadataWrapper<FlowMeterDto>(meters));
            }

            return NotFound();
        }

        [Route("{facilityid:int}/address")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilityAddress(int FacilityId)
        {
            List<Asset_Address> AddressList = null;
            AddressList = service.GetFacilityAddress(FacilityId);

            if (AddressList != null)
            {
                var addresses = ListAddress(AddressList);
                return Ok(new MetadataWrapper<AssetAddressDto>(addresses));
            }

            return NotFound();
        }


        [Route("{gatewayid:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPost]
        public IHttpActionResult SaveFacilityDetails(int GatewayId,[FromBody]AssetFacilityDetailsBindingModel[] facility)
        {
            HttpResponseMessage response = null;

                if (GatewayId <= 0 )
                {
                        return BadRequest();
                }

                if (facility == null)
                {
                         return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                        return BadRequest(ModelState);
                }
               

                List<Asset_FacilityDetails> facilities = new List<Asset_FacilityDetails>();

            foreach(AssetFacilityDetailsBindingModel f in facility )
            {
                var fac = new Asset_FacilityDetails();

                fac.GatewayId = GatewayId;
                fac.FacilityName = f.FacilityName;
                fac.FacilityAKA = f.FacilityAKA;
                fac.TypeId = f.TypeId;
                fac.OwnerId = f.OwnerId;
                fac.OwnerStatusId = f.OwnerStatusId;
                fac.StatusId = f.StatusId;
                fac.ClosedDate = f.ClosedDate;
                fac.ComplianceMgmtDate = f.ComplianceMgmtDate;
                fac.AnticipatedOpsDate = f.AnticipatedOpsDate;
                fac.EffectiveOpsDate = f.EffectiveOpsDate;
                fac.BillingTemplateId = f.BillingTemplateId;

                fac.Address1 = f.Address1;
                fac.Address2 = f.Address2;
                fac.CornerAddress = f.CornerAddress;
                fac.City = f.City;
                fac.CountryCode = f.CountryCode;
                fac.StateCode = f.StateCode;
                fac.CountyCode = f.CountyCode;
                fac.PostalCode = f.PostalCode;
                fac.TimeZoneCode = f.TimeZoneCode;

                fac.Latitude = f.Latitude.GetValueOrDefault();
                fac.Longitude = f.Longitude.GetValueOrDefault();
                fac.AppChangeUserId = f.AppChangeUserId;

                facilities.Add(fac);
            }
               if (service.SaveFacilityDetails(facilities) < 0)
                {
                  return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
                }
                else
                {
                return Ok();
                }

                

                   
        }


        [Route("{gatewayid:int}/{facilityid:int}")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
        [HttpPut]
        public IHttpActionResult UpdateFacilityDetails(int GatewayId, int FacilityId,[FromBody] AssetFacilityDetailsBindingModel[] facility)
        {
            HttpResponseMessage response = null;
           
                if (facility == null)
                {
                return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                return BadRequest(ModelState);
                }

                if(GatewayId <= 0 || FacilityId <= 0)
                {
                return BadRequest();
                }

                List<Asset_FacilityDetails> facilities = new List<Asset_FacilityDetails>();

            foreach (AssetFacilityDetailsBindingModel f in facility)
            {
                var fac = new Asset_FacilityDetails();

                fac.GatewayId = GatewayId;
                fac.FacilityId = FacilityId;
                fac.FacilityName = f.FacilityName;
                fac.FacilityAKA = f.FacilityAKA;
                fac.TypeId = f.TypeId;
                fac.OwnerId = f.OwnerId;
                fac.OwnerStatusId = f.OwnerStatusId;
                fac.StatusId = f.StatusId;
                fac.ClosedDate = f.ClosedDate;
                fac.ComplianceMgmtDate = f.ComplianceMgmtDate;
                fac.AnticipatedOpsDate = f.AnticipatedOpsDate;
                fac.EffectiveOpsDate = f.EffectiveOpsDate;
                fac.BillingTemplateId = f.BillingTemplateId;

                fac.Address1 = f.Address1;
                fac.Address2 = f.Address2;
                fac.CornerAddress = f.CornerAddress;
                fac.City = f.City;
                fac.CountryCode = f.CountryCode;
                fac.StateCode = f.StateCode;
                //fac.CountyId = f.CountyId.GetValueOrDefault();
                fac.CountyCode = f.CountyCode;
                fac.PostalCode = f.PostalCode;
                fac.TimeZoneCode = f.TimeZoneCode;

                fac.Latitude = f.Latitude.GetValueOrDefault();
                fac.Longitude = f.Longitude.GetValueOrDefault();
                fac.AppChangeUserId = f.AppChangeUserId;

                facilities.Add(fac);
            }
          

            if (service.SaveFacilityDetails(facilities) < 0)
            {
                return InternalServerError(new Exception(Resources.LangResource.DBErrorMessage));
            }
            else
            {
                return Ok();
            }

        }

        
        [Route("{facilityid:int}/Fuel")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilityFuel(int FacilityId)
        {
            List<Asset_FacilityFuel> FacilityFuelList = null;
            FacilityFuelList = service.GetFacilityFuel(FacilityId);

            if (FacilityFuelList != null)
            {
                return Ok(new MetadataWrapper<Asset_FacilityFuel>(FacilityFuelList));
            }

            return NotFound();
        }




    }
}
