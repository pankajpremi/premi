using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class UserGatewayDao : IUserGatewayDao
    {
        static UserGatewayDao()
        {
            Mapper.CreateMap<tblIdentity_UserGateways, UserGateway>();
            Mapper.CreateMap<UserGateway, tblIdentity_UserGateways>();
        }

        public List<UserGateway> FindByUserId(int userId, int portalId)
        {
            using (var context = new ApecIdentityEntities())
            {
                //check if user is ADMIN or SUPER ADMIN
                var user = (from u in context.AspNetUsers where u.Id == userId select u).FirstOrDefault();

                var userRole = user.AspNetUserRoles.FirstOrDefault();

                List<UserGateway> gateways = null;

                if (userRole != null)
                {

                    if (userRole.AspNetRole.Name == "SUPER ADMIN" || userRole.AspNetRole.Name == "ADMIN")
                    {
                        gateways = (from g in context.tblIdentity_Gateways
                                    join p in context.tblIdentity_Portals on g.PortalID equals p.ID
                                    where p.isActive == true
                                    select new UserGateway
                                    {
                                        UserId = userId,
                                        PortalId = g.PortalID,
                                        GatewayId = g.ID,
                                        PortalGatewayId = g.PortalGatewayID,
                                        GatewayName = g.Name,
                                        GatewayCode = g.Code
                                    }).ToList();
                    }
                    else
                    {
                        gateways = (from ug in context.tblIdentity_UserGateways
                                    join g in context.tblIdentity_Gateways on ug.GatewayId equals g.ID
                                    join p in context.tblIdentity_Portals on g.PortalID equals p.ID
                                    where ug.UserId == userId
                                    where p.isActive == true
                                    select new UserGateway
                                    {
                                        UserId = ug.UserId,
                                        PortalId = g.PortalID,
                                        GatewayId = ug.GatewayId,
                                        PortalGatewayId = g.PortalGatewayID,
                                        GatewayName = g.Name,
                                        GatewayCode = g.Code
                                    }).ToList();
                    }

                    return gateways;
                }

                return null;
            }
        }

        public int Insert(int userid, int gatewayId, int portalId)
        {
            using (var context = new ApecIdentityEntities())
            {

                var ug = (from g in context.tblIdentity_UserGateways where g.UserId == userid && g.GatewayId == gatewayId select g).FirstOrDefault();

                if (ug != null)
                {
                    return 0;
                }

                var entity = new UserGateway();
                entity.UserId = userid;
                entity.GatewayId = gatewayId;

                var newentity = Mapper.Map<UserGateway, tblIdentity_UserGateways>(entity);

                context.tblIdentity_UserGateways.Add(newentity);
                context.SaveChanges();

            }

            return 1;

        }

    }
}


