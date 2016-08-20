using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{

    // abstract factory interface. Creates data access objects.
    // ** GoF Design Pattern: Factory.

    public interface IDaoFactory
    {
        IUserDao UserDao { get; }
        IUserProfileDao UserProfileDao { get; }
        IUserClaimDao UserClaimDao { get; }
        IUserLoginDao UserLoginDao { get; }
        IRoleDao RoleDao { get; }
        IUserRoleDao UserRoleDao { get; }
        IUserSystemRoleDao UserSystemRoleDao { get; }
        IPortalDao PortalDao { get; }
        IUserPortalDao UserPortalDao { get; }
        IUserGatewayDao UserGatewayDao { get; }
        IClientDao ClientDao { get; }
        IRefreshTokenDao RefreshTokenDao { get; }
        IStatusDao StatusDao { get; }
        IPasswordPolicyDao PasswordPolicyDao {get;}
        IGatewaySystemRoleDao GatewaySystemRoleDao { get; }
        ISystemRoleDao SystemRoleDao { get; }
        IPermissionDao PermissionDao { get; }
        IUserFacilityDao UserFacilityDao { get; }

    }
}
