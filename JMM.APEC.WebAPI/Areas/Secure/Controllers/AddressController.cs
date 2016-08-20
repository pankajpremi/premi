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
    [RoutePrefix("api/v1/Addresses")]
    [Authorize]
    public class AddressController : SecureApiController
    {
        IService service { get; set; }

        public AddressController()
        {
            service = new Service(CurrentApiUser);
        }

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


        [Route("{addressId:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAddress(int addressId)
        {
            List<Asset_Address>AddressList = null;
            AddressList = service.GetAddress(addressId);

            if (AddressList != null)
            {
                var addresses = ListAddress(AddressList);

                return Ok(new MetadataWrapper<AssetAddressDto>(addresses));
            }

            return NotFound();
        }












    }
}
