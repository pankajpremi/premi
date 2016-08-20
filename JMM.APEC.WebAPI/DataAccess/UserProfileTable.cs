using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class UserProfileTable
    {

        private IdentityDatabase _database;
        private IUserProfileDao profileDao;

        public UserProfileTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            profileDao = factory.UserProfileDao;
        }

        /// <summary>
        /// Inserts a new user profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public int Insert(IdentityUserProfile profile)
        {
            UserProfile oUser = new UserProfile();
            oUser.ID = profile.Id;
            oUser.UserId = profile.UserId;
            oUser.TimeZoneCode = profile.TimeZoneCode;
            oUser.CompanyAccessRequest = profile.CompanyAccessRequest;
            oUser.CompanyName = profile.CompanyName;
            oUser.AccessRightRequest = profile.AccessRightRequest;
            oUser.TermsAccepted = profile.TermsAccepted;
            oUser.TermsAcceptedDateTime = profile.TermsAcceptedDateTime;

            return profileDao.Insert(oUser);
        }
    }
}