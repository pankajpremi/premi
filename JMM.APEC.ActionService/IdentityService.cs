using JMM.APEC.Core;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActionService
{
    public class IdentityService : IIdentityService
    {
        private ApplicationSystemUser currentUser = null;
        private IdentityDatabase database;
        private IDaoFactory factory = null;

        private IUserDao userDao;
        private IRoleDao roleDao;
        private IStatusDao statusDao;
        private IUserRoleDao userRoleDao;
        private IUserSystemRoleDao userSystemRoleDao;
        private IGatewaySystemRoleDao gatewaySystemRoleDao;
        private IPermissionDao permissionDao;
        private ISystemRoleDao systemRoleDao;
        private IUserFacilityDao userFacilityDao;

        public IdentityService(ApplicationSystemUser CurrentUser)
        {
            try
            {
                currentUser = CurrentUser;
                database = new IdentityDatabase();
                factory = database.GetFactory();

                userDao = factory.UserDao;
                roleDao = factory.RoleDao;
                statusDao = factory.StatusDao;
                userRoleDao = factory.UserRoleDao;
                userSystemRoleDao = factory.UserSystemRoleDao;
                gatewaySystemRoleDao = factory.GatewaySystemRoleDao;
                permissionDao = factory.PermissionDao;
                systemRoleDao = factory.SystemRoleDao;
                userFacilityDao = factory.UserFacilityDao;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public List<User> GetUsers(string gateways, string statuses, string searchText, int? pageNum, int? pageSize, string sortField, string sortDirection)
        {
            var users = userDao.GetUsers(currentUser, gateways, statuses, searchText, pageNum, pageSize, sortField, sortDirection);
            return users;
        }

        public User GetUser(int userId)
        {
            var cur = currentUser;

            var user = userDao.GetUserById(userId, cur.Portal.PortalId);
            return user;
        }

        public int UpdateUser(User user)
        {
            return userDao.UpdateWithProfile(user);
        }

        public int DeleteUser(int userId)
        {
            return userDao.Delete(userId);
        }

        public List<Role> GetRoles()
        {
            return roleDao.GetRoles();
        }

        public List<Permission> GetPermissions()
        {
            return permissionDao.GetPermissions();
        }

        public List<SystemRole> GetSystemRoles()
        {
            return systemRoleDao.GetSystemRoles();
        }

        public List<Status> GetStatuses()
        {
            return statusDao.FindStatuses();
        }

        public Status GetStatusForUser(int userId)
        {
            return statusDao.GetStatusByUserId(userId);
        }

        public Role GetRoleForUser(int userId)
        {
            return roleDao.GetRoleByUserId(userId);
        }

        public List<UserSystemRoleModel> GetSystemRolesForUser(int portalId, int gatewayId, int userId)
        {
            return userSystemRoleDao.FindByUserIdAndGateway(portalId, gatewayId, userId);
        }

        public List<UserFacilityModel> GetFacilitiesForUser(int portalId, int gatewayId, int userId)
        {
            return userFacilityDao.FindByUserIdAndGateway(portalId, gatewayId, userId);
        }

        public List<GatewaySystemRole> GetSystemRolesForGateway(int portalId, int userId)
        {
            return gatewaySystemRoleDao.FindByGatewayId(portalId, userId);
        }


    }
}
