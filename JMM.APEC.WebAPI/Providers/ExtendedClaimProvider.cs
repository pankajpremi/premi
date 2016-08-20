using JMM.APEC.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Web;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.WebAPI.Authorization;
using JMM.APEC.WebAPI.DataAccess;

namespace JMM.APEC.WebAPI.Providers
{
    public class ExtendedClaimProvider
    {
        public static IEnumerable<Claim> GetClaims(ApplicationIdentityUser user)
        {
            UserSystemRolesTable userRolesTable;
            IdentityDatabase database = new IdentityDatabase();

            List<Claim> claims = new List<Claim>();
            UserClaimsTable userClaimsTable;
            userClaimsTable = new UserClaimsTable(database);

            ClaimsIdentity identity = userClaimsTable.FindByUserId(user.Id);
            claims = identity.Claims.ToList();

            //add current portal as complex claim
            ApplicationPortal portal = (from p in user.Portals where p.CurrentPortal == true select p).FirstOrDefault();

            if (portal != null)
            {
                var p = new ComplexClaim<UserPortalClaim>("userportal", new UserPortalClaim(
                    portal.Id, portal.PortalPortalId, portal.Name, portal.Code, portal.CurrentDomain
                    ));
                claims.Add(p);

                //add user gateways for the selected portal as complex claims
                var gateways = (from g in user.Gateways where g.PortalId == portal.Id select g).ToList();
                foreach (var gateway in gateways)
                {
                    var g = new ComplexClaim<UserGatewayClaim>("usergateway", new UserGatewayClaim(
                        gateway.Id, gateway.PortalGatewayId, gateway.PortalId, gateway.Name, gateway.Code
                        ));
                    claims.Add(g);
                }

                //add system roles as complex claims
                userRolesTable = new UserSystemRolesTable(database);
                List<UserSystemRole> roles = userRolesTable.FindByUserId(user.Id, portal.Id);

                //limit roles to the current portal
                roles = (from r in roles where r.PortalId == portal.Id select r).ToList();

                foreach (var role in roles)
                {
                    var r = new ComplexClaim<UserSystemRoleClaim>("usersystemrole", new UserSystemRoleClaim(role.GatewayId, role.RoleId, role.PermissionId,
                        role.GatewayCode, role.RoleCode, role.PermissionCode, role.ServiceCode, role.PortalId
                        ));

                    claims.Add(r);
                }

            }

            return claims;

        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}