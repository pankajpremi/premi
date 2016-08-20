using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class PortalTable
    {
        private IdentityDatabase _database;
        private IPortalDao portalDao;

        public PortalTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            portalDao = factory.PortalDao;
        }

        public ApplicationPortal GetByPortalId(string portalId)
        {
            Portal oPortal = null;
            oPortal = portalDao.FindByPortalUrl(portalId);

            ApplicationPortal appPortal = null;

            if (oPortal != null)
            {
                appPortal = new ApplicationPortal();
                appPortal.Id = oPortal.ID;
                appPortal.PortalPortalId = oPortal.PortalPortalID;
                appPortal.Name = oPortal.Name;
                appPortal.DomainUrls = oPortal.DomainUrls.Split(',').ToList();
                appPortal.CurrentDomain = portalId;
                appPortal.CurrentPortal = true;
                appPortal.Active = (bool)oPortal.IsActive;
            }

            return appPortal;
        }

    }
}