//using JMM.APEC.BusinessObjects;
using JMM.APEC.Core;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace JMM.APEC.WebAPI.Helpers
{
    public static class IdentityHelper
    {
        public static List<UserClaim> ReadUserClaims(IEnumerable<Claim> claims, string username)
        {
            return claims.AsQueryable().Select(u =>
                         new UserClaim
                         {
                             //Id = u.Id,
                             UserName = username,
                             ClaimType = u.Type,
                             ClaimValue = u.Value
                         })
                         .ToList();
        }

        public static bool ValidatePortal(string username, ref ApplicationUserPortal portal)
        {
            IdentityDatabase database = null;
            database = new IdentityDatabase();

            UserPortalTable userPortalTable;
            userPortalTable = new UserPortalTable(database);

            var userPortal = userPortalTable.VerifyPortalforUser(username, portal.PortalId);

            if (userPortal != null)
            {
                portal.ConnectionName = userPortal.ConnectionName;
                portal.LocalPortalId = 1;
                return true;
            }

            return false;
        }
}
}