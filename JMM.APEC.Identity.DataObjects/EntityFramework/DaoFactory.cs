using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class DaoFactory : IDaoFactory
    {
        public IUserDao UserDao { get { return new UserDao(); } }
        public IUserProfileDao UserProfileDao { get { return new UserProfileDao(); } }
        public IUserClaimDao UserClaimDao { get { return new UserClaimDao(); } }
        public IUserLoginDao UserLoginDao { get { return new UserLoginDao();  } }
        public IRoleDao RoleDao { get { return new RoleDao(); } }
        public IUserRoleDao UserRoleDao { get { return new UserRoleDao(); } }
        public IUserSystemRoleDao UserSystemRoleDao { get { return new UserSystemRoleDao(); } }
        public IUserGatewayDao UserGatewayDao { get { return new UserGatewayDao(); } }
        public IPortalDao PortalDao { get { return new PortalDao(); } }
        public IUserPortalDao UserPortalDao { get { return new UserPortalDao();  } }
        public IClientDao ClientDao { get { return new ClientDao(); } }
        public IRefreshTokenDao RefreshTokenDao { get { return new RefreshTokenDao(); } }
        public IStatusDao StatusDao { get { return new StatusDao(); } }
        public IPasswordPolicyDao PasswordPolicyDao { get { return new PasswordPolicyDao(); } }
        public IGatewaySystemRoleDao GatewaySystemRoleDao { get { return new GatewaySystemRoleDao();  } }
        public IPermissionDao PermissionDao { get { return new PermissionDao(); } }
        public ISystemRoleDao SystemRoleDao { get { return new SystemRoleDao(); } }
        public IUserFacilityDao UserFacilityDao { get { return new UserFacilityDao(); } }

    }
}
