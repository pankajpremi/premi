using JMM.APEC.ActivityLog;
using JMM.APEC.ActivityLog.Interfaces;
using JMM.APEC.Alarm;
using JMM.APEC.Alarm.Interfaces;
using JMM.APEC.Calendar;
using JMM.APEC.Calendar.Interfaces;
using JMM.APEC.Common;
using JMM.APEC.Common.Interfaces;
using JMM.APEC.Core;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.Efile;
using JMM.APEC.Efile.Interfaces;
using JMM.APEC.EventTracker;
using JMM.APEC.EventTracker.Interfaces;
using JMM.APEC.IMS;
using JMM.APEC.IMS.Interfaces;
using JMM.APEC.ReleaseDetection;
using JMM.APEC.ReleaseDetection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JMM.APEC.ActionService
{
    public class Service : IService
    {
        private string provider = string.Empty;
        private ApecDatabase database = null;
        private IDaoFactory factory = null;

        private ApplicationSystemUser user = null;

        private ILocationDao LocationDao;

        private ISystemStatusDao SystemStatusDao;
        private ISystemCategoryDao SystemCategoryDao;
        private ISystemObjectDao SystemObjectDao;
        private ISystemTypeDao SystemTypeDao;

        private IAssetGatewayDao AssetGatewayDao;
        private IAssetPortalDao AssetPortalDao;
        private IAssetFacilityDao AssetFacilityDao;

        private IServiceAlarmDao ServiceAlarmDao;
        
        private ISystemUserLinkDao SystemUserLinkDao;

        private IServiceCalendarDao ServiceCalendarDao;

        private IEcommModuleDao EcommModuleDao;

        private IServiceEventTrackerDao ServiceEventTrackerDao;

        private IServiceActivityLogDao ServiceActivityLogDao;

        private IServiceFuelManagementDao ServiceFuelManagementDao;

        private IServiceFlowMeterDao ServiceFlowMetersDao;

        private IAssetSensorDao AssetSensorDao;

        private IServiceEfilesDao ServiceEfilesDao;

        private IServiceReleaseDetectionDao ServiceReleaseDetectionDao;

        private IAssetAddressDao AssetAddressDao;
               
        private ISystemMessageDao SystemMessageDao;
        private IAssetContactDao AssetContactDao;
        private IAssetPhoneDao AssetPhoneDao;
        private IAssetFacilityFuelDao AssetFacilityFuelDao;
       
       
        public Service(ApplicationSystemUser currentUser)
        {
            try {
                user = currentUser;
                provider = "ApecDataProvider";
                database = new ApecDatabase(provider, user);
                factory = database.GetFactory();

                LocationDao = factory.LocationDao;

                SystemCategoryDao = factory.SystemCategoryDao;
                SystemStatusDao = factory.SystemStatusDao;
                SystemObjectDao = factory.SystemObjectDao;
                SystemTypeDao = factory.SystemTypeDao;

                AssetGatewayDao = factory.AssetGatewayDao;
                AssetPortalDao = factory.AssetPortalDao;
                AssetFacilityDao = factory.AssetFacilityDao;

                ServiceAlarmDao = factory.ServiceAlarmDao;

                SystemUserLinkDao = factory.SystemUserLinkDao;

                EcommModuleDao = factory.EcommModuleDao;

                ServiceActivityLogDao = factory.ServiceActivityLogDao;

                ServiceEventTrackerDao = factory.ServiceEventTrackerDao;

                ServiceFuelManagementDao = factory.ServiceFuelManagementDao;

                ServiceFlowMetersDao = factory.ServiceFlowMeterDao;

                ServiceEfilesDao = factory.ServiceEfilesDao;

                AssetSensorDao = factory.AssetSensorDao;

                ServiceReleaseDetectionDao = factory.ServiceReleaseDetectionDao;

                ServiceCalendarDao = factory.ServiceCalendarDao;

                AssetAddressDao = factory.AssetAddressDao;

               
                SystemMessageDao = factory.SystemMessageDao;
                AssetContactDao = factory.AssetContactDao;
                AssetPhoneDao = factory.AssetPhoneDao;               
                AssetFacilityFuelDao = factory.AssetFacilityFuelDao;
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        // Module/Service Services
        public List<Ecomm_Module> GetModulesForUser()
        {
            var modules = EcommModuleDao.GetModulesForUser(user, null);

            return modules;
        }

        // TimeZone Services
        public List<System_TimeZone> GetTimeZone(int? TimeZoneId, string TimeZoneCode = "")
        {
            var timeZones = LocationDao.GetTimeZone(TimeZoneId, TimeZoneCode);

            return timeZones;
        }

        // State Services
        public List<System_State> GetState(string CountryCode, string StateCode)
        {
            var states = LocationDao.GetState(CountryCode, StateCode);

            return states;
        }

        // Country Services
        public List<System_Country> GetCountry(string CountryCode = "")
        {
            var timeZones = LocationDao.GetCountry(CountryCode);

            return timeZones;
        }

        // County Services
        public List<System_County> GetCounty(int? CountyId, string StateCode)
        {
            var counties = LocationDao.GetCounty(CountyId, StateCode);

            return counties;
        }

        // Status Services
        public List<System_StatusGateway> GetStatus(string GatewayId, string StatusTypeCode, string StatusCode)
        {
            var statuses = SystemStatusDao.GetStatus(user,GatewayId, StatusTypeCode, StatusCode);

            return statuses;
        }

        public List<System_Status> GetAlarmActionStatus()
        {
            var AlarmActionstatuses = SystemStatusDao.GetAlarmActionStatus();

            return AlarmActionstatuses;
        }

        public List<System_Status> GetAlarmResolutionStatus()
        {
            var AlarmResolutionstatuses = SystemStatusDao.GetAlarmResolutionStatus();

            return AlarmResolutionstatuses;
        }

       
        public void SaveStatusList(List<System_Status> statuses)
        {
            foreach (var s in statuses)
            {
                if (s.Id > 0)
                {
                    SystemStatusDao.UpdateStatus(s);
                }
                else
                {
                    SystemStatusDao.InsertStatus(s);
                }
            }

            return;
        }

        public void DeleteStatusList(List<System_Status> statuses)
        {
            foreach (var s in statuses)
            {
                SystemStatusDao.DeleteStatus(s);
            }

            return;
        }

        // Gateway Services
        public List<Asset_Gateway> GetGateway()
        {
            
            return null;
        }

        public List<Asset_Gateway> GetGatewaysForUser()
        {
            var gateways = AssetGatewayDao.GetGatewaysForUser(user);

            return gateways;
        }

        public List<Asset_Gateway> GetGateway(int? portalId, bool? active, int? gatewayId, string gatewaycode, int? gatewaystatuscode)
        {
            List<Asset_Gateway> gateways = null;
            gateways = AssetGatewayDao.GetGateway(portalId, active, gatewayId, gatewaycode, gatewaystatuscode);

            return gateways;
        }

   
        public Asset_Gateway SaveGateway(Asset_Gateway gateways)
        {
            if(gateways.Id > 0)
            {
                gateways =  AssetGatewayDao.UpdateGateway(gateways);                    
            }
            else
            {
                gateways = AssetGatewayDao.InsertGateway(gateways);                
            }

            return gateways;
        }
       

        public int SaveGatewayLocation(List<Asset_GatewayLocation> Locations)
        {
            int retval = 0;
            foreach (var loc in Locations)
            {
               
                if(AssetGatewayDao.SaveGatewayLocation(loc) < 0)
                {
                    retval = -1;
                }              
               
            }

            return retval;
        }

        public void DeleteGateway(Asset_Gateway gateways)
        {
           
           AssetGatewayDao.DeleteGateway(gateways);         

            return;
        }

   
        //Portal Services
        public List<Asset_Portal> GetPortal(int? PortalId, bool? Active)
        {
            var portals = AssetPortalDao.GetPortal(PortalId, Active);

            return portals;
        }

        public void SavePortal(List<Asset_Portal> portals)
        {
            foreach (var p in portals)
            {
                if (p.Id > 0)
                {
                    AssetPortalDao.UpdatePortal(p);
                }
                else
                {
                    AssetPortalDao.InsertPortal(p);
                }
            }

            return;
        }

        public void DeletePortal(List<Asset_Portal> portals)
        {
            foreach (var p in portals)
            {
                AssetPortalDao.DeletePortal(p);
            }

            return;
        }


        //Category Services
        public List<System_Category> GetCategoryList(int? GatewaylId, int? ObjectId, bool? Active)
        {
            var portals = SystemCategoryDao.GetCategories(GatewaylId, ObjectId, Active);

            return portals;
        }

        public void SaveCategoryItem(List<System_Category> categories)
        {
            foreach (var c in categories)
            {
                if (c.CategoryId > 0)
                {
                    SystemCategoryDao.UpdateCategory(c);
                }
                else
                {
                    SystemCategoryDao.InsertCategory(c);
                }
            }

            return;
        }

        public void DeleteCategoryItem(List<System_Category> categories)
        {
            foreach (var c in categories)
            {
                SystemCategoryDao.DeleteCategory(c);
            }

            return;
        }



        public void SaveObjectItem(List<System_Object> objects)
        {
            foreach (var o in objects)
            {
                if (o.Id > 0)
                {
                    SystemObjectDao.UpdateObject(o);
                }
                else
                {
                    SystemObjectDao.InsertObject(o);
                }
            }

            return;
        }

        public void DeleteObjectItem(List<System_Object> objects)
        {
            foreach (var o in objects)
            {
                SystemObjectDao.DeleteObject(o);
            }

            return;
        }


        //Type Services        
        public List<System_Type> GetTypeList(int? ObjectId, string ObjectCode, string GatewayIDs, string TypeCode)
        {
            var types = SystemTypeDao.GetType(user,ObjectId, ObjectCode, GatewayIDs, TypeCode);

            return types;
        }

        //public void SaveTypeItem(List<System_Type> types)
        //{
        //    foreach (var t in types)
        //    {
        //        if (t.TypeId > 0)
        //        {
        //            SystemTypeDao.UpdateType(t);
        //        }
        //        else
        //        {
        //            SystemTypeDao.InsertType(t);
        //        }
        //    }

        //    return;
        //}

        //public void DeleteTypeItem(List<System_Type> types)
        //{
        //    foreach (var t in types)
        //    {
        //        SystemTypeDao.DeleteType(t);
        //    }

        //    return;
        //}


        //Facility Services
        public List<Asset_Facility> GetFacility(int GatewayId, int? FacilityId, int? StatusId, int? TypeId,string GatewayCode, int? GatewayStatusCode,string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            var facilities = AssetFacilityDao.GetFacility(user,GatewayId, FacilityId, StatusId, TypeId,GatewayCode, GatewayStatusCode, Search, Seed, Limit, Sortcolumn, Sortorder);

            return facilities;
        }

        public List<Asset_Facility> GetTopFacilities()
        {
            var facilities = AssetFacilityDao.GetTopFacilities(user);

            return facilities;
        }

        public List<Asset_Facility> GetFacilityLatLong(int FacilityId)
        {
            var facilities = AssetFacilityDao.GetFacilityLatLong(FacilityId);

            return facilities;
        }

        public int InsertFacility(List<Asset_Facility> facility)
        {
            int retval = 0;
           foreach (Asset_Facility f in facility)
            {
                retval =  AssetFacilityDao.InsertFacility(f);
            }
               
            return retval;
        }

        public int UpdateFacility(List<Asset_Facility> facility)
        {

            int retval = 0;
            foreach (Asset_Facility f in facility)
            {
                retval = AssetFacilityDao.UpdateFacility(f);
            }

            return retval;
        }


        public int DeleteFacility(int FacilityId)
        {
            int retval = 0;
         
            retval = AssetFacilityDao.DeleteFacility(FacilityId);
            

            return retval;
        }

        public List<Asset_Contact> GetFacilityContactsByFacilityId(int FacilityId)
        {
            var facilitycontacts = AssetFacilityDao.GetFacilityContactsByFacilityId(user,FacilityId);

            return facilitycontacts.ToList();
        }

        public List<Asset_GatewayFacilityList> GetFacilities(int GatewayId, int? FacilityId, int? StatusId,int? TypeID, string GatewayCode, int? GatewayStatusId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            var facilitylist = AssetGatewayDao.GetFacilities(user, GatewayId, FacilityId, StatusId, TypeID, GatewayCode, GatewayStatusId, Search, Seed, Limit, Sortcolumn, Sortorder);

            if(facilitylist != null)
            {
                return facilitylist.ToList();
            }
            else
            {
                return null;
            }
        }

        //public List<Asset_FacilityDetails> GetFacilityDetails(int GatewayId, int FacilityId)
        //{
        //    var facilitylist = AssetGatewayDao.GetFacilityDetails(user, GatewayId, FacilityId);

        //    return facilitylist.ToList();
        //}

        public int SaveFacilityDetails(List<Asset_FacilityDetails> facilitydetails)
        {
            int retval = 0;
            foreach (Asset_FacilityDetails f in facilitydetails)
            {
                if(AssetFacilityDao.SaveFacilityDetails(f)< 0)
                {
                    retval = -1;
                }

            }
            return retval;
        }

      

        public List<Asset_FacilityFuel> GetFacilityFuel(int FacilityId)
        {
           var results = AssetFacilityFuelDao.GetFacilityFuel(FacilityId);
            return results;
        }

        //Alarm Services
        public List<Service_AlarmEvent> GetCriticalAlarmsList(string GatewayIds)
        {
            var criticalalarms = ServiceAlarmDao.GetCriticalAlarms(GatewayIds);

            return criticalalarms;
        }

        public List<Service_AlarmList> GetCriticalAlarmsForUser()
        {
            var criticalalarms = ServiceAlarmDao.GetCriticalAlarmsForUser(user);

            return criticalalarms;
        }

        public List<Service_AlarmList> GetAlarmsList(string Gateways, string Facilities, string statuses, string slas, DateTime? fromdate, DateTime? todate, string search, int? seed, int? limit, string sortcol, string sortorder)
        {
            var alarmsList = ServiceAlarmDao.GetAlarmsList(user,Gateways, Facilities, statuses, slas, fromdate, todate, search, seed, limit, sortcol, sortorder);

            return alarmsList;
        }

        public List<Service_AlarmInfo> GetAlarmById(int AlarmEventId)
        {
            var alarm = ServiceAlarmDao.GetAlarmById(AlarmEventId);

            return alarm;
        }

        public List<Service_AlarmDetails> GetAlarmDetailsById(int AlarmEventId)
        {
            var alarm = ServiceAlarmDao.GetAlarmDetailsById(AlarmEventId);

            return alarm;
        }


        public List<Service_AlarmEvent> GetPastSlaAlarmsList(int GatewayId)
        {
            var alarms = ServiceAlarmDao.GetPastSlaAlarmsList(GatewayId);

            return alarms;
        }

        public List<Service_AlarmEvent> GetPastSlaAlarmsForUser()
        {
            var alarms = ServiceAlarmDao.GetPastSlaAlarmsForUser(user);

            return alarms;
        }

        public List<Service_AlarmWorklog> GetAlarmWorkLogs(int AlarmEventId)
        {

            var AlarmWorkLogs = ServiceAlarmDao.GetAlarmWorkLogs(AlarmEventId);

            return AlarmWorkLogs;
        }


       
        public List<Asset_Facility> GetFacilityByAlarmId(int AlarmEventId)
        {
            var facility = ServiceAlarmDao.GetFacilityByAlarmId(AlarmEventId);

            return facility;
        }

        public List<Service_AlarmAtgEvents> GetAlarmAtgEvents(int AlarmEventId)
        {

            var alarmevents = ServiceAlarmDao.GetAlarmAtgEvents(AlarmEventId);

            return alarmevents;

        }

        //Link Services
        public List<System_UserLink> GetUserFavLinkList()
        {
            var favlinks = SystemUserLinkDao.GetFavLink(user);

            return favlinks;

        }

        //ActivityLog services
        public List<Service_ActivityLog> GetAllActivityLogs(string Gateways, string facilities, string ALType, DateTime? fromdate, DateTime? todate)
        {
            var ActivityLogs = ServiceActivityLogDao.GetAllActivityLogs(user,Gateways, facilities, ALType, fromdate, todate);

            return ActivityLogs;
        }

        public List<JMM.APEC.ActivityLog.Service_ActivityLogComment> GetActivityLogComments(int ActivityLogId)
        {
            var ActivityLogComments = ServiceActivityLogDao.GetActivityLogComments(ActivityLogId);

            return ActivityLogComments;
            
        }

        public List<JMM.APEC.ActivityLog.Service_ActivityLogMedia> GetActivityLogMedia(int ActivityLogId)
        {
            var ActivityLogMedia = ServiceActivityLogDao.GetActivityLogMedia(ActivityLogId);

            return ActivityLogMedia;
           
        }

        public List<Service_ActivityLog> GetActivityLogsByFacility(int FacilityId)
        {
            var ActivityLogs = ServiceActivityLogDao.GetActivityLogsByFacility(FacilityId);

            return ActivityLogs;
        }

        public List<System_Type> GetActivityLogTypes(int ObjectId, string GatewayIDs, string TypeCode)
        {
            var ActivityLogTypes = ServiceActivityLogDao.GetActivityLogTypes(user, ObjectId, GatewayIDs, TypeCode);

            return ActivityLogTypes;
           
        }

        //EventTracker Services
        public List<Service_EventTrackerReminderList> GetAllEventTrackerReminders(string Gateways, string Facilities, string Statuses, string Categorys, string Types, string SubTypes, DateTime? Fromdate, DateTime? Todate, int seed, int limit)
        {
            var eventreminders = ServiceEventTrackerDao.GetAllEventTrackerReminders(user, Gateways, Facilities, Statuses, Categorys,Types, SubTypes, Fromdate, Todate, seed, limit);

            return eventreminders;
        }

        public List<Service_EventTrackerReminder> GetEventTrackerRemindersByFacility(int FacilityId)
        {
            var eventreminders = ServiceEventTrackerDao.GetEventTrackerRemindersByFacility(FacilityId);

            return eventreminders;
        }

        //Tank Level Services
        public List<Service_TankCompartmentLevelGroup> GetLatestTankLevelsForDashboard(string Gateways, string Facilities, string Products, int? PercentLevel, int Seed, int Limit)
        {
            var tanklevels = ServiceFuelManagementDao.GetDashboardTankLevels(user, Gateways, Facilities, Products, PercentLevel, Seed, Limit);

            return tanklevels;
        }
        public List<Service_EventCompartmentLevel> GetLatestCompartmentTankLevelsForFacility(int? FacilityId)
        {
            var tanklevels = ServiceFuelManagementDao.GetFacilityCompartmentTankLevels_Latest(user, FacilityId);

            return tanklevels;
        }
        public List<Service_EventCompartmentLevel> GetCompartmentTankLevels(int? TankCompartmentId, int? Interval)
        {
            var tanklevels = ServiceFuelManagementDao.GetCompartmentTankLevels(user, null, null, null, TankCompartmentId, Interval, 0, 0);

            return tanklevels;
        }
        public List<Service_EventCompartmentDelivery> GetLatestCompartmentTankDelivery(int? TankCompartmentId)
        {
            var deliveries = ServiceFuelManagementDao.GetCompartmentTankDeliveries_Latest(user, TankCompartmentId);

            return deliveries;
        }
        public List<Service_TankCompartmentEvent> GetCompartmentTankEvents(int? TankCompartmentId, string EventTypes, int? Seed, int? Limit)
        {
            var events = ServiceFuelManagementDao.GetCompartmentTankEvents(user, TankCompartmentId, EventTypes, 0, 0);

            return events;
        }
   
        public List<Service_FlowMeterGroup> GetLatestFlowMetersForDashboard(string Gateways, string Facilities, decimal? GPMValue, int Seed, int Limit)
        {
            var flowmeters = ServiceFlowMetersDao.GetDashboardFlowMeters(user, Gateways, Facilities, GPMValue, Seed, Limit);

            return flowmeters;
        }
        public List<Service_FlowMeter> GetLatestFlowMetersForFacility(int? FacilityId)
        {
            var flowmeters = ServiceFlowMetersDao.GetFacilityFlowMeters_Latest(user, FacilityId);
            return flowmeters;
        }
        public List<Service_FlowMeter> GetFuelPositionFlowMeters(int? FuelPositionId, int? Interval)
        {
            var flowmeters = ServiceFlowMetersDao.GetFuelPositionFlowMeters(user, null, null, FuelPositionId, Interval, 0, 0);
            return flowmeters;
        }
        public List<Service_FlowMeterEvent> GetFuelPositionFlowMeterEvents(int? FuelPositionId, int? Seed, int? Limit)
        {
            var flowmeters = ServiceFlowMetersDao.GetFlowMeterEvents(user, FuelPositionId, 0, 0);
            return flowmeters;
        }

        //EFile Services
        public List<Service_EfileNode> GetEFileItems(string Gateways, string SortOption, string Keyword)
        {
            var efiles = ServiceEfilesDao.GetEFileItems(user, Gateways, SortOption, Keyword);

            return efiles;
        }

        public List<Asset_SensorAsset> GetSensorAssetDetails(int SensorId)
        {
            var sensorInfo = AssetSensorDao.GetSensorAssetDetails(SensorId);

            return sensorInfo;
        }

        public List<Service_ReleaseDetection> GetReleaseDetectionResults(string Gateways, string Facilities, string Results, string RDStatuses, string AssetTypes, DateTime? FromDate, DateTime? ToDate, int? ReleaseDetectionId, int Seed, int Limit)
        {
            var rdResults = ServiceReleaseDetectionDao.GetReleaseDetectionResults(user, Gateways, Facilities, Results, RDStatuses, AssetTypes, FromDate, ToDate, ReleaseDetectionId, Seed, Limit);

            return rdResults;
        }


        public List<System_Category> GetEventCategories(string GatewayIDs, int ObjectID)
        {
            var categories = ServiceEventTrackerDao.GetEventCategories(user,GatewayIDs, ObjectID);

            return categories;
        }
        public List<System_CategoryType> GetEventTypes(int ObjectId, string Categorys)
        {
            var types = ServiceEventTrackerDao.GetEventTypes(ObjectId, Categorys);

            return types;
        }
        public List<System_SubType> GetEventSubTypes(string GatewayIDs, int ObjectId, string Types)
        {
            var subtypes = ServiceEventTrackerDao.GetEventSubTypes(user,GatewayIDs, ObjectId, Types);

            return subtypes;
        }


        public List<Service_CalendarToDo> GetCalendarToDo(string Gateways, string Services, DateTime? Fromdate, DateTime? Todate, int seed, int limit)
        {
            var results = ServiceCalendarDao.GetCalendarToDo(user, Gateways, Services, Fromdate, Todate, seed, limit);

            return results;
        }

        public List<Asset_Address> GetFacilityAddress(int FacilityId)
        {
            var address = AssetFacilityDao.GetFacilityAddress(FacilityId);

            return address;
        }

        public List<System_MessageList> GetMessage(int? MessageId, string ObjectCode, string TypeCode, string UserToId, string UserFromId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            var Msg = SystemMessageDao.GetMessage(MessageId, ObjectCode, TypeCode,UserToId, UserFromId, Search, Seed, Limit, Sortcolumn, Sortorder);

            return Msg;
        }


        public List<System_MessageAssignment> GetMessageAssignment(string ObjectCode, string EntityId, string MessageId, string TypeId)
        {
            var Messageobjects = SystemMessageDao.GetMessageAssignment(ObjectCode, EntityId, MessageId, TypeId);

            return Messageobjects;
        }


        public System_MessageAssignmentList InsertMessageAssignment(System_MessageAssignmentList Message)
        {

            var messgassign = SystemMessageDao.InsertMessageAssignment(user, Message);
            return messgassign;
        }

        public System_MessageAssignmentList UpdateMessageAssignment(System_MessageAssignmentList Message)
        {
           var messgassign =  SystemMessageDao.UpdateMessageAssignment(user, Message);
            return messgassign;
        }


       public int DeleteMessage(int MessageId)
        {
            int retval = 0;

            if (SystemMessageDao.DeleteMessage(MessageId) < 0)
            {
                retval = -1;
            }

            return retval;

        }

        public System_Message SendUserMessage(int UserId, string Subject, string MessageText)
        {
            var Message = SystemMessageDao.InsertUserMessage(user, UserId, Subject, MessageText);

            return Message;
        }

        //address services

        public List<Asset_Address> GetAddress(int addressId)
        {
            var address = AssetAddressDao.GetAddress(addressId);
            return address;
        }

        //contact services
        public List<Asset_GatewayContactList> GetContact(int GatewayId, string Contacts, string ObjectCode, int? TypeID, bool? IsActive, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder)
        {
            var contacts = AssetContactDao.GetContact(GatewayId,Contacts, ObjectCode, TypeID, IsActive, Search, Seed, Limit, Sortcolumn, Sortorder);
            return contacts;
        }


        public Asset_ContactInformation SaveContactDetails(Asset_ContactInformation contactdetails)
        {
            var contact = AssetContactDao.SaveContactDetails(user, contactdetails);

            return contact;
            
        }













        //phone services

        public List<Asset_Phone> GetPhone(int GatewayId, string PhoneId)
        {
            var phones = AssetPhoneDao.GetPhone(GatewayId,PhoneId);

            return phones;
        }

        
        //Object Assignment Services
        public List<Asset_PhoneAssignment> GetPhoneList(string ObjectCode, string EntityId, string PhoneId, string TypeId)
        {
            var PhoneAssignments = AssetPhoneDao.GetPhoneAssignment(ObjectCode, EntityId, PhoneId, TypeId);

            return PhoneAssignments;
        }

        //public List<Asset_ObjectContact> GetContactList(string ObjectCode, string objectId, string EntityId, string TypeId)
        //{
        //    var Contactobjects = SystemObjectDao.GetContactList(ObjectCode, objectId, EntityId, TypeId);

        //    return Contactobjects;
        //}

    }
}
