using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class PortalDao : IPortalDao
    {
        static PortalDao()
        {
            Mapper.CreateMap<tblIdentity_Portals, Portal>();
            Mapper.CreateMap<Portal, tblIdentity_Portals>();
        }

        public Portal FindByPortalUrl(string portalId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var Portals = from u in context.tblIdentity_Portals where u.DomainURLs.Contains(portalId) select u;

                if (Portals != null)
                {
                    var SelectedPortals = Portals.AsQueryable().Select(u =>
                        new Portal
                        {
                            Code = u.Code,
                            Name = u.Name,
                            DatabaseName = u.DatabaseName,
                            ID = u.ID,
                            PortalPortalID = (int)u.PortalPortalID,
                            IsActive = u.isActive,
                            DomainUrls = u.DomainURLs

                        })
                        .ToList();

                    return SelectedPortals.FirstOrDefault();
                }

                return null;

            }
        }
    }
}
