using JMM.APEC.ActivityLog.Interfaces;
using JMM.APEC.Alarm.Interfaces;
using JMM.APEC.Calendar.Interfaces;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.Efile.Interfaces;
using JMM.APEC.EventTracker.Interfaces;
using JMM.APEC.IMS.Interfaces;
using JMM.APEC.ReleaseDetection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Common.Interfaces
{
    public interface IDaoFactory
    {
        IAssetPortalDao AssetPortalDao { get; }
        IAssetAddressDao AssetAddressDao { get; }
        IAssetFacilityDao AssetFacilityDao { get; }
        IAssetGatewayDao AssetGatewayDao { get; }
        IAssetPhoneDao AssetPhoneDao { get; }
        //ISystemApiLogEntryDao SystemApiLogEntryDao { get;}
        ILocationDao LocationDao { get; }
        ISystemStatusDao SystemStatusDao { get; }
        ISystemCategoryDao SystemCategoryDao { get; }
        ISystemObjectDao SystemObjectDao { get; }
        ISystemTypeDao SystemTypeDao { get; }
        IServiceAlarmDao ServiceAlarmDao { get; }
        ISystemUserLinkDao SystemUserLinkDao { get; }
       // ISystemServiceDao SystemServiceDao { get; }
        IEcommModuleDao EcommModuleDao { get; }

        IServiceActivityLogDao ServiceActivityLogDao { get; }

        IServiceEventTrackerDao ServiceEventTrackerDao { get; }
        IServiceFuelManagementDao ServiceFuelManagementDao { get; }
        IServiceFlowMeterDao ServiceFlowMeterDao { get; }
        IServiceEfilesDao ServiceEfilesDao { get; }
        IAssetSensorDao AssetSensorDao { get; }
        IServiceReleaseDetectionDao ServiceReleaseDetectionDao { get; }
        IServiceCalendarDao ServiceCalendarDao { get; }
        IEmailTemplateDao EmailTemplateDao { get; }
        ISystemMessageDao SystemMessageDao { get; }
        IAssetContactDao AssetContactDao { get; }
        IAssetFacilityFuelDao AssetFacilityFuelDao { get; }
        
    }
}
