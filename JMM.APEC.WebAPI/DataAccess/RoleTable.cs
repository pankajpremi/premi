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
    public class RoleTable
    {
        private IdentityDatabase _database;
        private IRoleDao roleDao;

        public RoleTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            roleDao = factory.RoleDao;
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(int roleId)
        {
            return roleDao.Delete(roleId);
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public int Insert(ApplicationIdentityRole role)
        {
            Role oRole = new Role();
            oRole.Id = role.Id;
            oRole.Name = role.Name;

            return roleDao.Insert(oRole);
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(int roleId)
        {
            ApplicationIdentityRole role = GetRoleById(roleId);

            string roleName = null;

            if(role != null)
            {
                roleName = role.Name;
            }

            return roleName;
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public int GetRoleId(string roleName)
        {
            ApplicationIdentityRole role = GetRoleByName(roleName);

            int roleId = 0;
            if(role != null)
            {
                roleId = role.Id;
            }

            return roleId;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ApplicationIdentityRole GetRoleById(int roleId)
        {
            Role oRole = null;
            oRole = roleDao.GetRoleById(roleId);

            ApplicationIdentityRole role = null;

            if (oRole != null)
            {
                role = new ApplicationIdentityRole(oRole.Name, oRole.Id);
            }

            return role;
        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public ApplicationIdentityRole GetRoleByName(string roleName)
        {

            Role oRole = null;
            oRole = roleDao.GetRoleByName(roleName);

            ApplicationIdentityRole role = null;

            if (oRole != null)
            {
                role = new ApplicationIdentityRole(oRole.Name, oRole.Id);
            }

            return role;
        }

        public int Update(ApplicationIdentityRole role)
        {
            Role oRole = new Role();

            oRole.Id = role.Id;
            oRole.Name = role.Name;

            return roleDao.Update(oRole);
        }
    }
}