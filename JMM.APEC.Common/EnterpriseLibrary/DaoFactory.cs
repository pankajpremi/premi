using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Common.Interfaces;
using JMM.APEC.Core.Interfaces;
using JMM.APEC.Core.EnterpriseLibrary;
using JMM.APEC.Alarm.Interfaces;
using JMM.APEC.Alarm.EnterpriseLibrary;
using JMM.APEC.IMS.Interfaces;
using JMM.APEC.IMS.EnterpriseLibrary;
using JMM.APEC.ReleaseDetection.Interfaces;
using JMM.APEC.ReleaseDetection.EnterpriseLibrary;
using JMM.APEC.Calendar.Interfaces;
using JMM.APEC.Calendar.EnterpriseLibrary;
using JMM.APEC.Efile.Interfaces;
using JMM.APEC.Efile.EnterpriseLibrary;
using JMM.APEC.ActivityLog.Interfaces;
using JMM.APEC.EventTracker.Interfaces;
using JMM.APEC.ActivityLog.EnterpriseLibrary;
using JMM.APEC.EventTracker.EnterpriseLibrary;

namespace JMM.APEC.Common.EnterpriseLibrary
{
    public class DaoFactory : IDaoFactory
    {
        private string _databaseName;

        public DaoFactory()
        {

        }

        public DaoFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public IAssetPortalDao AssetPortalDao { get { return new AssetPortalDao(_databaseName); } }
        public IAssetAddressDao AssetAddressDao { get { return new AssetAddressDao(_databaseName); } }
        public IAssetFacilityDao AssetFacilityDao { get { return new AssetFacilityDao(_databaseName); } }
        public IAssetGatewayDao AssetGatewayDao { get { return new AssetGatewayDao(_databaseName); } }
        public IAssetPhoneDao AssetPhoneDao { get { return new AssetPhoneDao(_databaseName); } }
        //public ISystemApiLogEntryDao SystemApiLogEntryDao { get { return new SystemApiLogEntryDao(); } }
        public ILocationDao LocationDao { get { return new LocationDao(_databaseName); } }
        public ISystemStatusDao SystemStatusDao { get { return new SystemStatusDao(_databaseName); } }
        public ISystemCategoryDao SystemCategoryDao { get { return new SystemCategoryDao(_databaseName); } }
        public ISystemObjectDao SystemObjectDao { get { return new SystemObjectDao(_databaseName); } }
        public ISystemTypeDao SystemTypeDao { get { return new SystemTypeDao(_databaseName); } }
        public IServiceAlarmDao ServiceAlarmDao { get { return new ServiceAlarmDao(_databaseName); } }
        public ISystemUserLinkDao SystemUserLinkDao { get { return new SystemUserLinkDao(_databaseName); } }
       // public ISystemServiceDao SystemServiceDao { get { return new SystemServiceDao(_databaseName); } }
        public IEcommModuleDao EcommModuleDao { get { return new EcommModuleDao(_databaseName); } }
        public IServiceActivityLogDao ServiceActivityLogDao { get { return new ServiceActivityLogDao(_databaseName); } }
        public IServiceEventTrackerDao ServiceEventTrackerDao { get { return new ServiceEventTrackerDao(_databaseName); } }
        public IServiceFuelManagementDao ServiceFuelManagementDao { get { return new ServiceFuelManagementDao(_databaseName); } }
        public IServiceFlowMeterDao ServiceFlowMeterDao { get { return new ServiceFlowMeterDao(_databaseName); } }
        public IAssetSensorDao AssetSensorDao { get { return new AssetSensorDao(_databaseName); } }
        public IServiceEfilesDao ServiceEfilesDao { get { return new ServiceEfilesDao(_databaseName); } }
        public IServiceReleaseDetectionDao ServiceReleaseDetectionDao { get { return new ServiceReleaseDetectionDao(_databaseName); } }
        public IServiceCalendarDao ServiceCalendarDao { get { return new ServiceCalendarDao(_databaseName); } }
        public IEmailTemplateDao EmailTemplateDao { get { return new EmailTemplateDao(_databaseName); } }
        public ISystemMessageDao SystemMessageDao { get { return new SystemMessageDao(_databaseName); } }
        public IAssetContactDao AssetContactDao { get { return new AssetContactDao(_databaseName); } }       
        public IAssetFacilityFuelDao AssetFacilityFuelDao { get { return new AssetFacilityFuelDao(_databaseName); } }
        

    }
}
