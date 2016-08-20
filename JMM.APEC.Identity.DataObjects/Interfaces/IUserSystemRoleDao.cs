using JMM.APEC.Core;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserSystemRoleDao
    {
        List<UserSystemRole> FindByUserId(int portalId, int userId);
        List<UserSystemRole> FindDistinctByUserId(int portalId, int userId);
        List<UserSystemRoleModel> FindByUserIdAndGateway(int portalId, int portalGatewayId, int userId);
        List<UserSystemRole> FindDistinctByUserIdAndGateway(int portalId, int gatewayId, int userId);
        int Delete(string userId);
        int Delete(User user, SystemRole role);
        int Insert(SystemRole role, string userId, int gatewayId, int portalId);

    }
}
