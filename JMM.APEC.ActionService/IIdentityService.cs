using JMM.APEC.Core;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActionService
{
    public interface IIdentityService
    {
        // User Repository

        List<User> GetUsers(string gateways, string statuses, string searchText, int? pageNum, int? pageSize, string sortField, string sortDirection);
        User GetUser(int userId);
        int UpdateUser(User user);
        int DeleteUser(int userId);

        List<Role> GetRoles();
        List<Permission> GetPermissions();
        List<SystemRole> GetSystemRoles();
        List<Status> GetStatuses();
        Status GetStatusForUser(int userId);
        Role GetRoleForUser(int userId);
        List<UserSystemRoleModel> GetSystemRolesForUser(int portalId, int gatewayId, int userId);
        List<UserFacilityModel> GetFacilitiesForUser(int portalId, int gatewayId, int userId);
        List<GatewaySystemRole> GetSystemRolesForGateway(int portalId, int userId);

    }
}
