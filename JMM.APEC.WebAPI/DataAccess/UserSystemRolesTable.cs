using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class UserSystemRolesTable
    {
        private IdentityDatabase _database;
        private IUserSystemRoleDao userRoleDao;

        public UserSystemRolesTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            userRoleDao = factory.UserSystemRoleDao;
        }

        public List<UserSystemRole> FindByUserId(int userId, int portalId)
        {
            var roles = userRoleDao.FindByUserId(portalId, userId);

            return roles;
        }

        public List<UserSystemRole> FindDistinctByUserId(int userId, int PortalId)
        {
            var roles = userRoleDao.FindDistinctByUserId(PortalId, userId);

            return roles;
        }

    }
}