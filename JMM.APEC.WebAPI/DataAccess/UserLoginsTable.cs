using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class UserLoginsTable
    {

        private IdentityDatabase _database;
        private IUserLoginDao userLoginDao;

        public UserLoginsTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            userLoginDao = factory.UserLoginDao;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public int Delete(ApplicationIdentityUser user, UserLoginInfo login)
        {
            User oUser = new User();
            oUser.Id = user.Id;
            oUser.UserName = user.UserName;

            UserLogin oLogin = new UserLogin();
            oLogin.UserId = user.Id;
            oLogin.LoginProvider = login.LoginProvider;
            oLogin.ProviderKey = login.ProviderKey;

            return userLoginDao.Delete(oUser, oLogin);
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(int userId)
        {
            return userLoginDao.Delete(userId);
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public int Insert(ApplicationIdentityUser user, UserLoginInfo login)
        {
            User oUser = new User();
            oUser.Id = user.Id;
            oUser.UserName = user.UserName;

            UserLogin oUserLogin = new UserLogin();
            oUserLogin.UserId = user.Id;
            oUserLogin.LoginProvider = login.LoginProvider;
            oUserLogin.ProviderKey = login.ProviderKey;

            return userLoginDao.Insert(oUser, oUserLogin);
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public int FindUserIdByLogin(UserLoginInfo userLogin)
        {
            UserLogin oUserLogin = new UserLogin();
            oUserLogin.ProviderKey = userLogin.ProviderKey;
            oUserLogin.LoginProvider = userLogin.LoginProvider;

            return userLoginDao.FindUserIdByLogin(oUserLogin);
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(int userId)
        {
            List<UserLogin> oLogins = null;
            oLogins = userLoginDao.FindByUserId(userId);

            List<UserLoginInfo> logins = new List<UserLoginInfo>();

            if (oLogins != null)
            {
                foreach (var oLogin in oLogins)
                {
                    UserLoginInfo login = new UserLoginInfo(oLogin.LoginProvider, oLogin.ProviderKey);
                    logins.Add(login);
                }
            }

            return logins;
      
        }
    }
}