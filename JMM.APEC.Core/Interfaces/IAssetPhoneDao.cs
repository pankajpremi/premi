using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JMM.APEC.Core.Interfaces
{
    public interface IAssetPhoneDao
    {
        List<Asset_Phone> GetPhone(int GatewayId, string PhoneId);
        int InsertPhone(Asset_Phone Phone);
        int UpdatePhone(Asset_Phone Phone);
        List<Asset_PhoneAssignment> GetPhoneAssignment(string Objectcode, string EntityId, string PhoneId, string TypeId);
        int SavePhoneAssignment(Asset_PhoneAssignmentEntity obj);
    }
}
