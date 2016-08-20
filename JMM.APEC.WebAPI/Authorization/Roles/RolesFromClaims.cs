using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.DataAccess;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    public class RolesFromClaims
    {
        public static IEnumerable<Claim> CreateRolesBasedOnClaims(ClaimsIdentity identity, int portalId)
        {
            UserSystemRolesTable userRolesTable;
            //get pre-defined APEC Roles
            List<Claim> claims = new List<Claim>();

            IdentityDatabase database = new IdentityDatabase();

            //get user info
            UserManager<ApplicationIdentityUser, int> manager = new UserManager<ApplicationIdentityUser, int>(new ApplicationUserStore<ApplicationIdentityUser, int>(database, portalId));
            ApplicationIdentityUser user = manager.FindByName(identity.Name);

            //create claims/roles based on System Roles for User Role only.
            if (identity.HasClaim(ClaimTypes.Role , "USER"))
            {
                userRolesTable = new UserSystemRolesTable(database);

                List<UserSystemRole> roles = userRolesTable.FindDistinctByUserId(user.Id, portalId);

                foreach (var role in roles)
                {
                    Claim roleClaim = new Claim(ClaimTypes.Role, role.RoleCode);
                    claims.Add(roleClaim);
                }
            }

            return claims;
        }
    }
}