using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;

namespace JMM.APEC.WebAPI.DataAccess
{

    public class UserTable<TUser>
        where TUser : ApplicationIdentityUser
    {
        private IdentityDatabase _database;
        private IUserDao userDao;
        private IUserGatewayDao gatewayDao;
        private IUserPortalDao portalDao;
        private IStatusDao statusDao;

        public UserTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            userDao = factory.UserDao;
            portalDao = factory.UserPortalDao;
            gatewayDao = factory.UserGatewayDao;
            statusDao = factory.StatusDao;
        }

         private TUser ObjectToObject(User oUser)
         {
             TUser user = (TUser)Activator.CreateInstance(typeof(TUser));

             user.Id = oUser.Id;
             user.UserName = oUser.UserName;
             user.FirstName = oUser.FirstName;
             user.LastName = oUser.LastName;
             user.Address1 = oUser.Address1;
             user.Address2 = oUser.Address2;
             user.City = oUser.City;
             user.CountryCode = oUser.Country;
             user.StateCode = oUser.State;
             user.PostalCode = oUser.PostalCode;
             user.PasswordHash = string.IsNullOrEmpty(oUser.PasswordHash) ? null : oUser.PasswordHash;
             user.SecurityStamp = string.IsNullOrEmpty(oUser.SecurityStamp) ? null : oUser.SecurityStamp;
             user.Email = string.IsNullOrEmpty(oUser.Email) ? null : oUser.Email;
             user.EmailConfirmed = oUser.EmailConfirmed;
             user.PhoneNumber = string.IsNullOrEmpty(oUser.PhoneNumber) ? null : oUser.PhoneNumber;
             user.PhoneNumberConfirmed = oUser.PhoneNumberConfirmed;
             user.LockoutEnabled = oUser.LockoutEnabled;
             user.LockoutEndDateUtc = oUser.LockoutEndDateUtc;
             user.AccessFailedCount = oUser.AccessFailedCount;
             user.StatusCode = oUser.StatusCode;
             user.StatusUpdateDateTime = oUser.StatusUpdateDateTime;
             user.Approved = oUser.Approved;
             user.SignUpDate = oUser.SignUpDate;

             return user;
         }

    /// <summary>
    /// Returns the user's name given a user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public string GetUserName(int userId)
        {
            return userDao.GetUserName(userId);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public int GetUserId(string userName)
        {
            return userDao.GetUserId(userName);
        }

        public List<ApplicationGateway> GetUserGateways(int UserId, int PortalId)
        {
            //get user gateways
            List<UserGateway> gateways = gatewayDao.FindByUserId(UserId, PortalId);

            if (gateways != null)
            {

                var UserGateways = gateways.AsQueryable().Select(u =>
                    new ApplicationGateway
                    {
                        Name = u.GatewayName,
                        Id = u.GatewayId,
                        Code = u.GatewayCode,
                        PortalGatewayId = u.PortalGatewayId,
                        PortalId = u.PortalId
                    })
                    .ToList();

                return UserGateways;
            }

            return null;
        }

        public List<ApplicationPortal> GetUserPortals(int UserId, int PortalId)
        {
            //get user portals
            List<UserPortal> portals = portalDao.FindByUserIdAndPortal(UserId, PortalId);

            if (portals != null)
            {
                var UserPortals = portals.AsQueryable().Select(u =>
                    new ApplicationPortal
                    {
                        Name = u.PortalName,
                        Id = u.PortalId,
                        PortalPortalId = u.PortalPortalId,
                        CurrentPortal = true,
                        CurrentDomain = "",
                        Active = (bool)u.IsActive,
                        Code = u.PortalCode,
                        DomainUrls = u.DomainUrls.Split(',').ToList()
                    })
                    .ToList();

                return UserPortals;
            }

            return null;
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(int userId)
        {
            User oUser = null;
            oUser = userDao.GetUserById(userId, 1);

            TUser user = null;

            if (oUser != null)
            {
                user = ObjectToObject(oUser);
            }

            return user;
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            List<User> oUsers = null;
            oUsers = userDao.GetUserByName(userName);

            List<TUser> users = new List<TUser>();

            if (oUsers != null)
            {
                foreach (var oUser in oUsers)
                {
                    TUser user = ObjectToObject(oUser); 

                    users.Add(user);
                }
            }

            return users;
        }

        public List<TUser> GetUserByEmail(string email)
        {
            List<User> oUsers = null;
            oUsers = userDao.GetUserByEmail(email);

            List<TUser> users = new List<TUser>();

            if (oUsers != null)
            {
                foreach (var oUser in oUsers)
                {
                    TUser user = ObjectToObject(oUser);

                    users.Add(user);
                }
            }

            return users;

        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(int userId)
        {
            return userDao.GetPasswordHash(userId);
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(int userId, string passwordHash)
        {
            return userDao.SetPasswordHash(userId, passwordHash);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(int userId)
        {
            return userDao.GetSecurityStamp(userId);
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            User oUser = new User();
            oUser.UserName = user.UserName;
            oUser.FirstName = user.FirstName;
            oUser.LastName = user.LastName;
            oUser.Address1 = user.Address1;
            oUser.Address2 = user.Address2;
            oUser.City = user.City;
            oUser.State = user.StateCode;
            oUser.PostalCode = user.PostalCode;
            oUser.Country = user.CountryCode;
            oUser.Id = user.Id;
            oUser.PasswordHash = user.PasswordHash;
            oUser.SecurityStamp = user.SecurityStamp;
            oUser.Email = user.Email;
            oUser.EmailConfirmed = user.EmailConfirmed;
            oUser.SignUpDate = user.SignUpDate;
            oUser.PhoneNumber = user.PhoneNumber;
            oUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            oUser.AccessFailedCount = user.AccessFailedCount;
            oUser.LockoutEnabled = user.LockoutEnabled;
            oUser.LockoutEndDateUtc = user.LockoutEndDateUtc ?? (DateTime)SqlDateTime.MinValue;
            oUser.TwoFactorEnabled = user.TwoFactorEnabled;

            Status CurrentStatus = statusDao.FindStatusByCode(user.StatusCode);
            //lookup user status
            oUser.StatusId = CurrentStatus.Id;
            oUser.StatusUpdateDateTime = user.StatusUpdateDateTime;

            int UserId = userDao.Insert(oUser);

            user.Id = UserId;

            return UserId;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(int userId)
        {
            return userDao.Delete(userId);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {

            User oUser = new User();

            oUser.Id = user.Id;
            oUser.UserName = user.UserName;
            oUser.FirstName = user.FirstName;
            oUser.LastName = user.LastName;
            oUser.Address1 = user.Address1;
            oUser.Address2 = user.Address2;
            oUser.City = user.City;
            oUser.State = user.StateCode;
            oUser.PostalCode = user.PostalCode;
            oUser.Country = user.CountryCode;
            oUser.PasswordHash = user.PasswordHash;
            oUser.SecurityStamp = user.SecurityStamp;
            oUser.Email = user.Email;
            oUser.EmailConfirmed = user.EmailConfirmed;
            oUser.PhoneNumber = user.PhoneNumber;
            oUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            oUser.AccessFailedCount = user.AccessFailedCount;
            oUser.LockoutEnabled = user.LockoutEnabled;
            oUser.LockoutEndDateUtc = user.LockoutEndDateUtc ?? (DateTime)SqlDateTime.MinValue;
            oUser.TwoFactorEnabled = user.TwoFactorEnabled;
            oUser.Approved = user.Approved;

            Status CurrentStatus = statusDao.FindStatusByCode(user.StatusCode);
            //lookup user status
            oUser.StatusId = CurrentStatus.Id;
            oUser.StatusUpdateDateTime = user.StatusUpdateDateTime;

            return userDao.Update(oUser);
        }

    }
}
