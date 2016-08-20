using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JMM.APEC.Core.Interfaces
{
    public interface IAssetGatewayDao
    {
         List<Asset_Gateway> GetGatewaysForUser(ApplicationSystemUser user);
         List<Asset_Gateway> GetGateway(int? PortalId, bool? IsPortalActive, int? GatewayId, string GatewayCode, int? GatewayStatusCode);
        Asset_Gateway InsertGateway(Asset_Gateway gateway);
        Asset_Gateway UpdateGateway(Asset_Gateway gateway);
        void DeleteGateway(Asset_Gateway gateway);
        int SaveGatewayLocation(Asset_GatewayLocation Location);
        List<Asset_GatewayFacilityList> GetFacilities(ApplicationSystemUser user,int GatewayId, int? FacilityId, int? StatusId, int? TypeId, string GatewayCode, int? GatewayStatusId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        List<Asset_FacilityDetails> GetFacilityDetails(ApplicationSystemUser user, int GatewayId, int FacilityId);
    }
}
