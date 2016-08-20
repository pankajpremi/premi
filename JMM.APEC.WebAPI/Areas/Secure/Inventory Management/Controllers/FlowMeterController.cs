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
        [RoutePrefix("api/v1/flowmeters")]
        [Authorize(Roles = "SUPER ADMIN,ADMIN,FLWMTR")]
        public class FlowMeterController : SecureApiController
        {
            IService service { get; set; }

            public FlowMeterController()
            {
                service = new Service(CurrentApiUser);
            }

            public FlowMeterController(IService service) { this.service = service; }

            private List<FlowMeterGroupDto> ListFlowMetersGroup(List<Service_FlowMeterGroup> flotMeterList)
            {
                var flowMeters = from t in flotMeterList
                                 select new FlowMeterGroupDto()
                                 {
                                     AvgGPM = t.AvgGPM,
                                     FacilityId = t.FacilityId,
                                     FacilityName = t.FacilityName,
                                     GatewayId = t.GatewayId,
                                     GatewayName = t.GatewayName,
                                     MinGPM = t.MinGPM,
                                     NumOfDispensers = t.NumOfDispensers,
                                     NumOfTransactions = t.NumOfTransactions
                                     
                                 };

                return flowMeters.ToList();
            }

            private List<FlowMeterDto> ListFlowMeters(List<Service_FlowMeter> flotMeterList)
            {
                var flowMeters = from t in flotMeterList
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

                return flowMeters.ToList();
            }

            private List<FlowMeterEventDto> ListFlowMeterEvents(List<Service_FlowMeterEvent> flotMeterList)
            {
                var flowMeters = from t in flotMeterList
                                 select new FlowMeterEventDto()
                                 {
                                    AvgGPMValue = t.AvgGPMValue,
                                    NumOfTransactions = t.NumOfTransactions,
                                    TotalVolumeAmt = t.TotalVolumeAmt,
                                    ReadingDateTime = t.ReadingDateTime

                                 };

                return flowMeters.ToList();
            }


        [Route("mainflowmeters")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetMainFlowMeters([FromUri]FlowMeterBindingModel model)
        {
            List<Service_FlowMeterGroup> flowMeterList = null;

            if (model != null)
            {
                flowMeterList = service.GetLatestFlowMetersForDashboard(model.Gateways, model.Facilities, model.GPMLevel, 0, 0);
            }
            else
            {
                flowMeterList = service.GetLatestFlowMetersForDashboard(null, null, null, 0, 0);
            }

            if (flowMeterList != null)
            {
                var flowmeters = ListFlowMetersGroup(flowMeterList);

                return Ok(new MetadataWrapper<FlowMeterGroupDto>(flowmeters));
            }

            return NotFound();

        }

        [Route("{fuelpositionid:int}/intervals/{days:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFlowMetersForFuelPosition(int? FuelPositionId, int? Days)
        {
            List<Service_FlowMeter> flowMeterList = null;
            flowMeterList = service.GetFuelPositionFlowMeters(FuelPositionId, Days);

            if (flowMeterList != null)
            {
                var statuses = ListFlowMeters(flowMeterList);

                return Ok(new MetadataWrapper<FlowMeterDto>(statuses));
            }

            return NotFound();

        }

        [Route("{fuelpositionid:int}/events")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetEventsForTankCompartment(int? FuelPositionId)
        {
            List<Service_FlowMeterEvent> meterEventList = null;
            meterEventList = service.GetFuelPositionFlowMeterEvents(FuelPositionId, 0, 0);

            if (meterEventList != null)
            {
                var statuses = ListFlowMeterEvents(meterEventList);

                return Ok(new MetadataWrapper<FlowMeterEventDto>(statuses));
            }

            return NotFound();
        }

    }
    }

