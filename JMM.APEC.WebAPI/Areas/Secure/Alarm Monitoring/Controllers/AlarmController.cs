using JMM.APEC.ActionService;
//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.WebAPI.Areas.Secure.Models;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JMM.APEC.Alarm;
using JMM.APEC.Core;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/alarms")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,ALARM")]
     public class AlarmController : SecureApiController
    {

        IService service { get; set; }

        public AlarmController()
        {
            service = new Service(CurrentApiUser);
        }

        public AlarmController(IService service) { this.service = service; }

        private List<ServiceAlarmInfoDto> ListAlarmsInfo(List<Service_AlarmInfo> AlarmSlaList)
        {
            var Alarm = from t in AlarmSlaList
                            select new ServiceAlarmInfoDto()
                            {
                                AlarmId = t.AlarmId,
                                AlarmCode = t.AlarmCode,
                                AlarmEventId = t.AlarmEventId,
                                AlarmName = t.AlarmName,
                                GatewayId = t.GatewayId,
                                GatewayName = t.GatewayName,
                                FacilityName = t.FacilityName,
                                SLAId = t.SLAId,
                                SLADue = t.SLADue,
                                Status = t.Status                   

                            };

            return Alarm.ToList();
        }

        private List<ServiceAlarmDetailsDto> ListAlarmsDetails(List<Service_AlarmDetails> AlarmList)
        {
            var Alarm = from t in AlarmList
                        select new ServiceAlarmDetailsDto()
                        {
                            AlarmId = t.AlarmId,
                            AlarmCode = t.AlarmCode,
                            AlarmEventId = t.AlarmEventId,
                            AlarmName = t.AlarmName,
                            GatewayId = t.GatewayId,
                            SensorId = t.SensorId,
                            Sensor = t.Sensor,
                            AlarmDateTime = t.AlarmDateTime,
                            SLAId = t.SLAId,
                            SLADue = t.SLADue,
                            StatusId = t.StatusId,
                            Status = t.Status,
                            Asset = "N/A",
                            Category = "Actionable"
                        };

            return Alarm.ToList();
        }

        private List<AssetFacilityAddressInfoDto> ListfacilityInfo(List<Asset_Facility> facilityList)
        {
            var facilityMap = from t in facilityList
                              select new AssetFacilityAddressInfoDto()
                              {
                                  FacilityId = t.Id,
                                  FacilityName = t.Name,
                                  FacilityAkaName = t.AKAName,
                                  Address1 = t.Address.Address1,
                                  Address2 = t.Address.Address2,
                                  City = t.Address.City,
                                  Zip = t.Address.PostalCode,
                                  State = t.Address.State.Name,
                                  Phone = "(720)-324-2260"
                              };

            return facilityMap.ToList();
        }

        private List<ServiceAlarmAtgEventDto> ListAtgEvents(List<Service_AlarmAtgEvents> eventsList)
        {
            var events = from t in eventsList
                              select new ServiceAlarmAtgEventDto()
                              {
                                 Type = t.Type,
                                 Time = t.Time,
                                 Event = t.Event,
                                 Information = t.Information
                              };

            return events.ToList();
        }

        private List<ServiceAlarmListDto> ListAlarms(List<Service_AlarmList> AlarmList)
        {
            var Alarms = from t in AlarmList
                         select new ServiceAlarmListDto()
                         {
                             AlarmEventId = t.AlarmEventId,
                             ActiveAlarmsCount = t.ActiveAlarmsCount,
                             GatewayName = t.GatewayName,
                             FacilityName = t.FacilityName,
                             FacilityId = t.FacilityId,
                             gatewayId = t.gatewayId,
                             JMMStatus = t.JMMStatus,
                             AlarmCategory = t.AlarmCategory,
                             AlarmType = t.AlarmType,
                             Sensor = t.Sensor,
                             SensorId = t.SensorId,                           
                             Duration = string.Format("{0:%d}day {0:%h}hr {0:%m}min", TimeSpan.FromSeconds(t.DurationInSeconds))

                         };

            return Alarms.ToList();
        }


        private List<ServiceAlarmEventDto> ListAlarmsEvents(List<Service_AlarmEvent> AlarmList)
        {
            var Alarms = from t in AlarmList
                         select new ServiceAlarmEventDto()
                         {
                             AlarmEventId = t.AlarmEventId                                           

                            };

            return Alarms.ToList();
        }

        private List<ServiceAlarmWorklogDto> ListAlarmWorkLogs(List<Service_AlarmWorklog> WorkLogList)
        {
            var worklogs = from t in WorkLogList
                           select new ServiceAlarmWorklogDto()
                           {
                               CommentId = t.CommentId,
                               AlarmEventId = t.AlarmEventId,
                               Comment = t.Comment,
                               CommentDateTime = t.CommentDateTime,
                               EnteredBy = t.EnteredBy

                           };

            return worklogs.ToList();
        }

        [Route("critical")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetCriticalAlarms( )
        {
            List<Service_AlarmList> AlarmsList = null;
            AlarmsList = service.GetCriticalAlarmsForUser();

            if (AlarmsList != null)
            {
                var Alarms = ListAlarms(AlarmsList);

                return  Ok(new MetadataWrapper<ServiceAlarmListDto>(Alarms)); 
            }

            return NotFound();
        }


        [Route("pastslas")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAlarmsPastSLA()
        {
            List<Service_AlarmEvent> AlarmsSlaList = null;
            AlarmsSlaList = service.GetPastSlaAlarmsForUser();

            if (AlarmsSlaList != null)
            {
                var AlarmSlas = ListAlarmsEvents(AlarmsSlaList);

                return Ok(new MetadataWrapper<ServiceAlarmEventDto>(AlarmSlas));
            }

            return NotFound();

        }


        [Route("")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAlarmsList([FromUri]ServiceAlarmBindingModel model)
        {
            List<Service_AlarmList> AlarmsList = null;

            if(model != null)
            {
                AlarmsList = service.GetAlarmsList(model.Gateways, model.Facilities, model.Statuses, model.slas, model.FromDate, model.ToDate,model.SearchText,model.PageNum, model.PageSize, null, null);
            }
            else
            {
                AlarmsList = service.GetAlarmsList(null,null,null,null, null,null,null,0,0, null, null);
            }

           

            if (AlarmsList != null)
            {
                var Alarms = ListAlarms(AlarmsList);

                
                return Ok(new MetadataWrapper<ServiceAlarmListDto>(Alarms));
            }

            return NotFound();

        }


        [Route("{alarmeventid:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAlarm(int alarmeventId)
        {
            
           List<Service_AlarmInfo> AlarmsList = new List<Service_AlarmInfo>();

            AlarmsList = service.GetAlarmById(alarmeventId);

           if (AlarmsList != null)
            {
                var Alarms = ListAlarmsInfo(AlarmsList);

                return Ok(new MetadataWrapper<ServiceAlarmInfoDto>(Alarms));

            }

            return NotFound();

        }

        [Route("{alarmeventid:int}/details")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAlarmDetails(int alarmeventId)
        {
                    
            List<Service_AlarmDetails> AlarmsList = new List<Service_AlarmDetails>();
            AlarmsList = service.GetAlarmDetailsById(alarmeventId);
           
            if (AlarmsList != null)
            {
                var Alarms = ListAlarmsDetails(AlarmsList);

                return Ok(new MetadataWrapper<ServiceAlarmDetailsDto>(Alarms));

            }

            return NotFound();

        }


        [Route("{alarmeventid:int}/facility")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetFacilityByAlarmId(int alarmeventId)
        {
             List<Asset_Facility> facList = new List<Asset_Facility>();

            facList = service.GetFacilityByAlarmId(alarmeventId);           

            if (facList != null)
            {
                var facilities = ListfacilityInfo(facList);

                return Ok(new MetadataWrapper<AssetFacilityAddressInfoDto>(facilities));

            }

            return NotFound();

        }

        [Route("{alarmeventid:int}/events")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAtgEventsByAlarmId(int alarmeventId)
        {
            List<Service_AlarmAtgEvents> eventList = new List<Service_AlarmAtgEvents>();

            eventList = service.GetAlarmAtgEvents(alarmeventId);

            if (eventList != null)
            {
                var events = ListAtgEvents(eventList);

                return Ok(new MetadataWrapper<ServiceAlarmAtgEventDto>(events));

            }

            return NotFound();

        }


        [Route("{alarmeventid:int}/worklogs")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetWorkLogListByAlarm(int AlarmEventId)
        {
            List<Service_AlarmWorklog> AlarmWorkLogList = null;
            AlarmWorkLogList = service.GetAlarmWorkLogs(AlarmEventId);

            if (AlarmWorkLogList != null)
            {
                var AlarmWorkLogs = ListAlarmWorkLogs(AlarmWorkLogList);

                return Ok(new MetadataWrapper<ServiceAlarmWorklogDto>(AlarmWorkLogs));
                
            }

            return NotFound();
        }

        private List<SensorAssetDto> ListSensor(List<Asset_SensorAsset> Senlist)
        {
            var sensor = from t in Senlist
                         select new SensorAssetDto()
                         {
                             SensorId = t.SensorId,
                             SensorLabel = t.SensorLabel,
                             Asset = t.Asset,
                             AtgName = t.AtgName
                         };

            return sensor.ToList();
        }

        [Route("{alarmeventid:int}/sensor")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetSensorAssetInfo(int AlarmEventId)
        {
            List<Asset_SensorAsset> senlist = null;
            senlist = service.GetSensorAssetDetails(AlarmEventId);

            if (senlist != null)
            {
                var sensorInfo = ListSensor(senlist);

                return Ok(new MetadataWrapper<SensorAssetDto>(sensorInfo));

            }

            return NotFound();
        }






    }
}
