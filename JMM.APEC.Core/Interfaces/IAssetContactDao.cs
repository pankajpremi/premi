using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface IAssetContactDao
    {
        List<Asset_GatewayContactList> GetContact(int GatewayId, string Contacts, string ObjectCode, int? TypeID, bool? IsActive, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        Asset_Contact InsertContact(Asset_Contact Contact);
        Asset_Contact UpdateContact(Asset_Contact Contact);
        int DeleteContact(int ContactId, int AppChangeUserId);
        List<Asset_ContactAssignment> GetContactAssignment(string ObjectCode, string EntityId, string ContactId, string TypeId);
        int SaveContactAssignmentByContact(Asset_ContactAssignmentByContact obj);
        Asset_ContactInformation SaveContactDetails(ApplicationSystemUser user,Asset_ContactInformation contactdetails);
    }
}
