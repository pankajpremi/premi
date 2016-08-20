using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class UserRolesTable
    {

        private IdentityDatabase _database;
        private IUserRoleDao userRoleDao;

        public UserRolesTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            userRoleDao = factory.UserRoleDao;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(int userId)
        {
            var roles = userRoleDao.FindByUserId(userId);

            return roles;
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(int userId)
        {
            return userRoleDao.Delete(userId);
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(ApplicationIdentityUser user, int roleId)
        {
            User oUser = new User();
            oUser.Id = user.Id;
            oUser.UserName = user.UserName;

            return userRoleDao.Insert(oUser, roleId);
        }
    }
}