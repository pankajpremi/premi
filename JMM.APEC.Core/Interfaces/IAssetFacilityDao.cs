using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JMM.APEC.Core.Interfaces
{
    public interface IAssetFacilityDao
    {
        List<Asset_Facility> GetFacility(ApplicationSystemUser user,int GatewayId, int? FacilityId, int? StatusId,int? TypeId, string GatewayCode, int? GatewayStatusCode, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        List<Asset_Facility> GetTopFacilities(ApplicationSystemUser user);
        List<Asset_Facility> GetFacilityLatLong(int FacilityId);
        int InsertFacility(Asset_Facility facility);
        int UpdateFacility(Asset_Facility facility);
        int DeleteFacility(int facilityId);
        List<Asset_Contact> GetFacilityContactsByFacilityId(ApplicationSystemUser user,int FacilityId);
        List<Asset_Address> GetFacilityAddress(int FacilityId);
        int SaveFacilityDetails(Asset_FacilityDetails facility);
        //int UpdateFacilityDetails(int GatewayId, int FacilityId, Asset_FacilityDetails facility);
    }
}
