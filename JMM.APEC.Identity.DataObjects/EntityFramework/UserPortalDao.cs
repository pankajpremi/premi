using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class UserPortalDao : IUserPortalDao
    {
        static UserPortalDao()
        {
            Mapper.CreateMap<tblIdentity_UserPortals, UserPortal>();
            Mapper.CreateMap<UserPortal, tblIdentity_UserPortals>();
        }

        public List<UserPortal> FindByUserName(string userName)
        {
            using (var context = new ApecIdentityEntities())
            {
                var user = (from ur in context.AspNetUsers where ur.UserName == userName select ur).FirstOrDefault();

                if (user != null)
                {
                    var portals = from u in context.tblIdentity_UserPortals
                                  join p in context.tblIdentity_Portals on u.PortalId equals p.ID
                                  where u.UserId == user.Id
                                  select new UserPortal
                                  {
                                      PortalName = p.Name,
                                      PortalCode = p.Code,
                                      UserId = u.UserId,
                                      PortalId = p.ID,
                                      IsActive = p.isActive,
                                      DomainUrls = p.DomainURLs,
                                      ConnectionName = p.DatabaseName
                                  };


                    return portals.ToList();
                }
            }

            return null;
        }

        public List<UserPortal> FindByUserId(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var portals = from u in context.tblIdentity_UserPortals
                              join p in context.tblIdentity_Portals on u.PortalId equals p.ID
                              where u.UserId == userId
                              select new UserPortal
                              {
                                  PortalName = p.Name,
                                  PortalCode = p.Code,
                                  UserId = u.UserId,
                                  PortalId = p.ID,
                                  IsActive = p.isActive,
                                  DomainUrls = p.DomainURLs,
                                  ConnectionName = p.DatabaseName
                              };


                return portals.ToList();
            }
        }

        public List<UserPortal> FindByUserIdAndPortal(int userId, int portalId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var portals = from u in context.tblIdentity_UserPortals
                              join p in context.tblIdentity_Portals on u.PortalId equals p.ID
                              where u.UserId == userId
                              where p.isActive == true
                              where p.ID == portalId
                              select new UserPortal
                              {
                                  PortalName = p.Name,
                                  PortalCode = p.Code,
                                  UserId = u.UserId,
                                  PortalId = p.ID,
                                  PortalPortalId = (int)p.PortalPortalID,
                                  IsActive = p.isActive,
                                  DomainUrls = p.DomainURLs,
                                  ConnectionName = p.DatabaseName
                              };


                return portals.ToList();
            }
        }

        public List<UserPortal> FindByUserIdAndPortalPortal(int userId, int portalPortalId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var portals = from u in context.tblIdentity_UserPortals
                              join p in context.tblIdentity_Portals on u.PortalId equals p.ID
                              where u.UserId == userId
                              where p.isActive == true
                              where p.ID == portalPortalId
                              select new UserPortal
                              {
                                  PortalName = p.Name,
                                  PortalCode = p.Code,
                                  UserId = u.UserId,
                                  PortalId = p.ID,
                                  PortalPortalId = (int)p.PortalPortalID,
                                  IsActive = p.isActive,
                                  DomainUrls = p.DomainURLs,
                                  ConnectionName = p.DatabaseName
                              };


                return portals.ToList();
            }
        }

        public int Insert(int userId, int portalId)
        {
            try {
                using (var context = new ApecIdentityEntities())
                {
                    var entity = new tblIdentity_UserPortals();
                    entity.UserId = userId;
                    entity.PortalId = portalId;
                    entity.CreateDateTime = DateTime.Now;

                    context.tblIdentity_UserPortals.Add(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return 0;
            }


            return 1;

        }

    }
}


