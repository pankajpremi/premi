using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class UserPortalTable
    {
        private IdentityDatabase _database;
        private IUserPortalDao userPortalDao;

        public UserPortalTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            userPortalDao = factory.UserPortalDao;
        }

        public UserPortal VerifyPortalforUser(string UserName, int PortalId)
        {
            UserPortal portal = null;
            var portals = userPortalDao.FindByUserName(UserName);

            if (portals != null && portals.Count > 0)
            {
                portal = portals[0];
            }

            return portal;
        }
   
        public int Insert(int UserId, int PortalId)
        {
            return userPortalDao.Insert(UserId, PortalId);
        }
    }
}