//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.Alarm;
using JMM.APEC.Calendar;
using JMM.APEC.Core;
using JMM.APEC.Efile;
using JMM.APEC.IMS;
using JMM.APEC.ReleaseDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActionService
{
    // single interface to all 'repositories'

    public interface IService
    {
        // Location Repository
        List<System_TimeZone> GetTimeZone(int? TimeZoneId, string TimeZoneCode = "");

        List<System_State> GetState(string CountryCode, string StateCode);

        List<System_Country> GetCountry(string CountryCode = "");

        List<System_County> GetCounty(int? CountyId, string StateCode);

        //void SaveStatusList(List<System_Status> statuses);
        //void DeleteStatusList(List<System_Status> statuses);
        List<System_StatusGateway> GetStatus(string GatewayIds, string StatusTypeCode, string StatusCode);

        List<Asset_Gateway> GetGatewaysForUser();
        List<Asset_Gateway> GetGateway(int? portalId, bool? active, int? gatewayId, string gatewaycode, int? gatewaystatuscode);
        Asset_Gateway SaveGateway(Asset_Gateway gateways);
        void DeleteGateway(Asset_Gateway gateways);

        List<Asset_Facility> GetFacility(int GatewayId, int? FacilityId, int? StatusId, int? TypeID,string GatewayCode, int? GatewayStatusId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        //List<Asset_Facility> GetFacilityLatLong(int FacilityId);
        List<Asset_Facility> GetTopFacilities();
        int InsertFacility(List<Asset_Facility> facility);
        int UpdateFacility(List<Asset_Facility> facility);
        int DeleteFacility(int FacilityId);

        List<Asset_Portal> GetPortal(int? PortalId, bool? Active);
        void SavePortal(List<Asset_Portal> portals);
        void DeletePortal(List<Asset_Portal> portals);

        List<System_Category> GetCategoryList(int? GatewayId, int? ObjectId, bool? Active);
        //void SaveCategoryItem(List<System_Category> categories);
        //void DeleteCategoryItem(List<System_Category> categories);

        //List<System_Object> GetObjectList(string ObjectCode);
        void SaveObjectItem(List<System_Object> objects);
        void DeleteObjectItem(List<System_Object> objects);

        List<System_Type> GetTypeList(int? ObjectId, string ObjectCode, string GatewayIDs, string TypeCode);
        //void SaveTypeItem(List<System_Type> Type);
        //void DeleteTypeItem(List<System_Type> Type);
        List<Ecomm_Module> GetModulesForUser();
        List<Service_AlarmEvent> GetCriticalAlarmsList(string GatewayIds);
        List<Service_AlarmList> GetCriticalAlarmsForUser();
        List<Service_AlarmList> GetAlarmsList(string Gateways, string Facilities, string statuses, string slas, DateTime? fromdate, DateTime? todate, string search,int? seed, int? limit, string sortcolumn, string sortorder);
        List<Service_AlarmInfo> GetAlarmById(int AlarmEventId);
        List<Service_AlarmWorklog> GetAlarmWorkLogs(int AlarmEventId);
        //Service_Comment GetAlarmWorkLogById(int AlarmEventId, int WorkLogId);
        List<Service_AlarmEvent> GetPastSlaAlarmsList(int GatewayId);
        List<Service_AlarmEvent> GetPastSlaAlarmsForUser();

        List<System_UserLink> GetUserFavLinkList();

        //List<System_GatewayService> GetGatewayServicesList(System_SearchCriteria SearchCriteria);

        List<Asset_Contact> GetFacilityContactsByFacilityId(int FacilityId);

        List<JMM.APEC.ActivityLog.Service_ActivityLog> GetAllActivityLogs(string Gateways, string facilities, string ALTypes, DateTime? fromdate, DateTime? todate);
        List<JMM.APEC.ActivityLog.Service_ActivityLogComment> GetActivityLogComments(int ActivityLogId);
        List<JMM.APEC.ActivityLog.Service_ActivityLogMedia> GetActivityLogMedia(int ActivityLogId);
        List<JMM.APEC.ActivityLog.Service_ActivityLog> GetActivityLogsByFacility(int FacilityId);
        List<System_Type> GetActivityLogTypes(int ObjectId, string GatewayIDs, string TypeCode);

        List<JMM.APEC.EventTracker.Service_EventTrackerReminderList> GetAllEventTrackerReminders(string Gateways, string Facilities, string Statuses, string Categorys, string Types, string SubTypes, DateTime? Fromdate, DateTime? Todate, int seed, int limit);

        List<JMM.APEC.EventTracker.Service_EventTrackerReminder> GetEventTrackerRemindersByFacility(int FacilityId);

        List<Service_TankCompartmentLevelGroup> GetLatestTankLevelsForDashboard(string Gateways, string Facilities, string Products, int? PercentLevel, int Seed, int Limit);
        List<Service_EventCompartmentLevel> GetLatestCompartmentTankLevelsForFacility(int? FacilityId);
        List<Service_EventCompartmentLevel> GetCompartmentTankLevels(int? TankCompartmentId, int? Interval);
        List<Service_TankCompartmentEvent> GetCompartmentTankEvents(int? TankCompartmentId, string EventTypes, int? Seed, int? Limit);

        List<Service_FlowMeterGroup> GetLatestFlowMetersForDashboard(string Gateways, string Facilities, decimal? GPMValue, int Seed, int Limit);
        List<Service_FlowMeter> GetLatestFlowMetersForFacility(int? FacilityId);
        List<Service_FlowMeter> GetFuelPositionFlowMeters(int? FuelPositionId, int? Interval);
        List<Service_FlowMeterEvent> GetFuelPositionFlowMeterEvents(int? FuelPositionId, int? Seed, int? Limit);

        List<Service_EfileNode> GetEFileItems(string Gateways, string SortOption, string Keyword);

        List<Asset_SensorAsset> GetSensorAssetDetails(int SensorId);

        List<Service_ReleaseDetection> GetReleaseDetectionResults(string Gateways, string Facilities, string Results, string RDStatuses, string AssetTypes, DateTime? Fromdate, DateTime? Todate, int? ReleaseDetectionId, int Seed, int Limit);

        List<System_Status> GetAlarmActionStatus();
        List<System_Status> GetAlarmResolutionStatus();
        List<Service_AlarmDetails> GetAlarmDetailsById(int AlarmEventId);
        List<Asset_Facility> GetFacilityByAlarmId(int AlarmEventId);
        List<Service_AlarmAtgEvents> GetAlarmAtgEvents(int AlarmEventId);

        List<System_Category> GetEventCategories(string GatewayIds, int ObjectID);
        List<System_CategoryType> GetEventTypes(int ObjectId, string Categorys);
        List<System_SubType> GetEventSubTypes(string GatewayIds, int ObjectId, string Types);

        List<Service_CalendarToDo> GetCalendarToDo(string Gateways, string Services, DateTime? Fromdate, DateTime? Todate, int seed, int limit);

        List<Asset_Address> GetFacilityAddress(int FacilityId);
        int SaveGatewayLocation(List<Asset_GatewayLocation> Locations);
        List<Asset_GatewayFacilityList> GetFacilities(int GatewayId, int? FacilityId, int? StatusId,int? TypeId, string GatewayCode, int? GatewayStatusIdstring, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        //List<Asset_FacilityDetails> GetFacilityDetails(int GatewayId, int FacilityId);
        int SaveFacilityDetails(List<Asset_FacilityDetails> facilitydetails);

        List<System_MessageList> GetMessage(int? MessageId, string ObjectCode, string TypeCode, string UserToId, string UserFromId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        System_MessageAssignmentList InsertMessageAssignment(System_MessageAssignmentList Message);
        System_MessageAssignmentList UpdateMessageAssignment(System_MessageAssignmentList Message);
        int DeleteMessage(int MessageId);

        System_Message SendUserMessage(int UserId, string Subject, string Message);

        List<Asset_Address> GetAddress(int addressId);
        List<Asset_GatewayContactList> GetContact(int GatewayId, string Contacts, string ObjectCode, int? TypeID, bool? IsActive, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        List<Asset_Phone> GetPhone(int GatewayId, string PhoneId);
        List<Asset_FacilityFuel> GetFacilityFuel(int FacilityId);
        List<Asset_PhoneAssignment> GetPhoneList(string ObjectCode, string EntityId, string PhoneId, string TypeId);
        //List<Asset_ObjectContact> GetContactList(string ObjectCode, string objectId, string EntityId, string TypeId);
        List<System_MessageAssignment> GetMessageAssignment(string ObjectCode, string objectId, string EntityId, string TypeId);

        Asset_ContactInformation SaveContactDetails(Asset_ContactInformation contactdetails);

    }
}
