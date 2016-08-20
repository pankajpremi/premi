using JMM.APEC.ActionService;
using JMM.APEC.IMS;
//using JMM.APEC.BusinessObjects;
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
    [RoutePrefix("api/v1/tanklevels")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,TNKLVL")]
    public class TankLevelController : SecureApiController
    {
        IService service { get; set; }

        public TankLevelController()
        {
            service = new Service(CurrentApiUser);
        }

        public TankLevelController(IService service) { this.service = service; }

        private List<TankLevelGroupDto> ListTankLevelsGroup(List<Service_TankCompartmentLevelGroup> tankLevelList)
        {
            var tankLevels = from t in tankLevelList
                             select new TankLevelGroupDto()
                            {
                                 GatewayId = t.GatewayId,
                                 GatewayName = t.GatewayName,
                                 FacilityId = t.FacilityId,
                                 FacilityName = t.FacilityName,
                                 NumOfTankComparments = (int)t.NumOfTanks,
                                 MinLevelPercent = t.MinLevelPercent,
                                 AvgLevelPercent = t.AvgLevelPercent,
                                 MaxWater = t.MaxWater,
                                 AvgWater = t.AvgWater
                            };

            return tankLevels.ToList();
        }

        private List<TankEventDto> ListTankComparmentEvents(List<Service_TankCompartmentEvent> tankEventList)
        {
            var tankEvents = from t in tankEventList
                             select new TankEventDto()
                             {
                                 TypeName = t.TypeName,
                                 EventDate = t.EventDateTime,
                                 HeightAmnt = t.HeightAmt,
                                 VolumeAmnt = t.VolumeAmt
                             };

            return tankEvents.ToList();
        }

        private List<TankLevelDto> ListCompartmentTankLevels(List<Service_EventCompartmentLevel> tankLevelList)
        {
            var tankLevels = from t in tankLevelList
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
                                 WaterHeightAmt = t.WaterHeightAmt
                             };

            return tankLevels.ToList();
        }

        [Route("maintanklevels")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetMainTankLevels([FromUri]TankLevelsBindingModel model)
        {
            List<Service_TankCompartmentLevelGroup> tankLevelList = null;

            if (model != null)
            {
                tankLevelList = service.GetLatestTankLevelsForDashboard(model.Gateways, model.Facilities, model.Products, model.PercentLevel, 0, 0);
            }
            else
            {
                tankLevelList = service.GetLatestTankLevelsForDashboard(null, null, null, null, 0, 0);
            }

            if (tankLevelList != null)
            {
                var tankLevels = ListTankLevelsGroup(tankLevelList);

                return Ok(new MetadataWrapper<TankLevelGroupDto>(tankLevels));
            }

            return NotFound();

        }

        [Route("{tankcompartmentid:int}/intervals/{days:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetLevelsForTankCompartment(int? TankCompartmentId, int? Days)
        {
            List<Service_EventCompartmentLevel> tankLevelList = null;
            tankLevelList = service.GetCompartmentTankLevels(TankCompartmentId, Days);

            if (tankLevelList != null)
            {
                var statuses = ListCompartmentTankLevels(tankLevelList);

                return Ok(new MetadataWrapper<TankLevelDto>(statuses));
            }

            return NotFound();

        }

        [Route("{tankcompartmentid:int}/events")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetEventsForTankCompartment(int? TankCompartmentId, [FromUri]TankCompartmentEventBidingModel model)
        {
            List<Service_TankCompartmentEvent> tankEventList = null;
            tankEventList = service.GetCompartmentTankEvents(TankCompartmentId, model.Types, 0, 0);

            if (tankEventList != null)
            {
                var statuses = ListTankComparmentEvents(tankEventList);

                return Ok(new MetadataWrapper<TankEventDto>(statuses));
            }

            return NotFound();
        }

    }
}
