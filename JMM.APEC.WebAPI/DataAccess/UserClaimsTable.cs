using JMM.APEC.Identity.DataObjects;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JMM.APEC.Identity.BusinessObjects;
using System.Configuration;
using JMM.APEC.WebAPI.Infrastructure;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class UserClaimsTable
    {

        private IdentityDatabase _database;
        private IUserClaimDao userClaimDao;

        public UserClaimsTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            userClaimDao = factory.UserClaimDao;
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(int userId)
        {
 
            List<UserClaim> oClaims = null;
            oClaims = userClaimDao.FindByUserId(userId);

            ClaimsIdentity claims = new ClaimsIdentity();

            if (oClaims != null)
            {
                foreach (var oClaim in oClaims)
                {
                    Claim claim = new Claim(oClaim.ClaimType, oClaim.ClaimValue);
                    claims.AddClaim(claim);
                }
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(int userId)
        {
            return userClaimDao.Delete(userId);
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(ApplicationIdentityUser user, Claim claim)
        {
            User oUser = new User();
            oUser.Id = user.Id;
            oUser.UserName = user.UserName;

            UserClaim oClaim = new UserClaim();
            oClaim.UserId = user.Id;
            oClaim.ClaimType = claim.Type;
            oClaim.ClaimValue = claim.Value;

            return userClaimDao.Delete(oUser, oClaim);
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, int userId)
        {
            UserClaim oUserClaim = new UserClaim();
            oUserClaim.ClaimType = userClaim.Type;
            oUserClaim.ClaimValue = userClaim.Value;
            oUserClaim.UserId = userId;

            return userClaimDao.Insert(oUserClaim, userId);
        }


    }
}
