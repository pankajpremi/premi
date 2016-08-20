using JMM.APEC.ActionService;
using JMM.APEC.Core;
using JMM.APEC.WebAPI.Areas.Public.Models;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Public.Controllers
{
    [RoutePrefix("api/v1/locations")]
    public class LocationController : BaseApiController
    {
        IService service { get; set; }

        public LocationController()
        {
            service = new Service(null);
        }

        private List<SystemTimeZoneDto> ListTimeZones(List<System_TimeZone> timeZoneList)
        {
            var timeZones = from t in timeZoneList
                            select new SystemTimeZoneDto()
                            {
                                Id = t.Id,
                                Code = t.Code,
                                Name = t.Name,
                                GMT = t.GMT,
                                Offset = t.Offset
                            };

            return timeZones.ToList();
        }

        private List<SystemStateDto> ListStates(List<System_State> stateList)
        {
            var states = from t in stateList
                         select new SystemStateDto()
                         {
                             StateId = t.Id,
                             StateCode = t.Code,
                             StateName = t.Name,
                             CountryId = t.Country.Id,
                             CountryCode = t.Country.Code,
                             CountryName = t.Country.Name
                         };

            return states.ToList();
        }

        private List<SystemCountryDto> ListCountries(List<System_Country> countryList)
        {
            var countries = from t in countryList
                            select new SystemCountryDto()
                            {
                                CountryId = t.Id,
                                Code = t.Code,
                                Name = t.Name,
                            };

            return countries.ToList();
        }

        private List<SystemCountyDto> ListCounties(List<System_County> countyList)
        {
            var counties = from t in countyList
                           select new SystemCountyDto()
                           {
                               CountyId = t.Id,
                               CountyName = t.Name,
                               StateId = t.State.Id,
                               StateCode = t.State.Code,
                               StateName = t.State.Name
                           };

            return counties.ToList();
        }

        //[Route("timezones")]
        //[AllowAnonymous]
        //[HttpGet]
        //public IHttpActionResult GetTimeZones()
        //{
        //    List<System_TimeZone> timeZoneList = null;
        //    timeZoneList = service.GetTimeZone(null, null);

        //    if (timeZoneList != null)
        //    {
        //        var timeZones = ListTimeZones(timeZoneList);

        //        return Ok(new MetadataWrapper<SystemTimeZoneDto>(timeZones));
        //    }

        //    return NotFound();
        //}

        [Route("timezones")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult GetTimeZone([FromUri]TimeZoneModel model)
        {
            List<System_TimeZone> timeZoneList = null;

            if(model != null)
            { 
                timeZoneList = service.GetTimeZone(model.Id, model.Code);                
            }
            else
            {
                timeZoneList = service.GetTimeZone(null, null);
            }

            if (timeZoneList != null)
            {
                var timeZones = ListTimeZones(timeZoneList);

                return Ok(new MetadataWrapper<SystemTimeZoneDto>(timeZones));
            }

            return NotFound();
        }

        //[Route("states")]
        //[AllowAnonymous]
        //[HttpGet]
        //public IHttpActionResult GetStates()
        //{
        //    List<System_State> stateList = null;
        //    stateList = service.GetState(null, null);

        //    if (stateList != null)
        //    {
        //        var states = ListStates(stateList);
        //        return Ok(new MetadataWrapper<SystemStateDto>(states));
        //    }

        //    return NotFound();
        //}

        [Route("states")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult GetState([FromUri]StateModel model)
        {
            List<System_State> stateList = null;

            if (model != null)
            {
                stateList = service.GetState(model.CountryCode, model.StateCode);                
            }
            else
            {
                stateList = service.GetState(null, null);
            }

            if (stateList != null)
            {
                var states = ListStates(stateList);
                return Ok(new MetadataWrapper<SystemStateDto>(states));
            }

            return NotFound();
        }

        //[Route("countries")]
        //[AllowAnonymous]
        //[HttpGet]
        //public IHttpActionResult GetCountries()
        //{
        //    List<System_Country> countryList = null;
        //    countryList = service.GetCountry(null);

        //    if (countryList != null)
        //    {
        //        var countries = ListCountries(countryList);

        //        return Ok(new MetadataWrapper<SystemCountryDto>(countries));

        //    }

        //    return NotFound();
        //}

        [Route("countries")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult GetCountry([FromUri]CountryModel model)
        {
            List<System_Country> countryList = null;

            if (model != null)
            {
                countryList = service.GetCountry(model.Code);                
            }
            else
            {
                countryList = service.GetCountry(null);
            }

            if (countryList != null)
            {
                var countries = ListCountries(countryList);

                return Ok(new MetadataWrapper<SystemCountryDto>(countries));
            }

            return NotFound();
        }

        //[Route("counties")]
        //[AllowAnonymous]
        //[HttpGet]
        //public IHttpActionResult GetCounties()
        //{
        //    List<System_County> countyList = null;
        //    countyList = service.GetCounty(null, null);

        //    if (countyList != null)
        //    {
        //        var counties = ListCounties(countyList);

        //        return Ok(new MetadataWrapper<SystemCountyDto>(counties));
        //    }

        //    return NotFound();
        //}

        [Route("counties")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult GetCounty([FromUri]CountyModel model)
        {
            List<System_County> countyList = null;
            if (model != null)
            {
                countyList = service.GetCounty(model.Id, model.StateCode);                
            }
            else
            {
                countyList = service.GetCounty(null, null);
            }

            if (countyList != null)
            {
                var counties = ListCounties(countyList);

                return Ok(new MetadataWrapper<SystemCountyDto>(counties));
            }

            return NotFound();
        }

    }
}
