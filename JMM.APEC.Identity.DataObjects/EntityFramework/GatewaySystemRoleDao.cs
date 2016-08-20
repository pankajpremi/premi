using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class GatewaySystemRoleDao : IGatewaySystemRoleDao
    {
        static GatewaySystemRoleDao()
        {

        }

        public List<GatewaySystemRole> FindByGatewayId(int PortalId, int GatewayId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var roles = from gr in context.tblIdentity_GatewaySystemRoles
                            join r in context.tblIdentity_SystemRoles on gr.SystemRoleId equals r.ID
                            join g in context.tblIdentity_Gateways on gr.GatewayId equals g.ID
                            join p in context.tblIdentity_Portals on g.PortalID equals p.ID
                            where p.PortalPortalID == PortalId
                            where g.PortalGatewayID == GatewayId
                            where r.Active == true
                            where r.isDeleted == false
                            select new GatewaySystemRole
                            {
                                GatewayCode = g.Code,
                                GatewayId = g.ID,
                                GatewayPortalId = (int)g.PortalGatewayID,
                                PortalId = g.PortalID,
                                GatewayName = g.Name,
                                RoleCode = r.Code,
                                RoleId = r.ID,
                                RoleName = r.Name,
                                ServiceCode = r.ServiceCode,
                                Permissions = (from rp in context.tblIdentity_SystemRolePermissions
                                               join p in context.tblIdentity_Permissions on rp.PermissionId equals p.Id
                                               where rp.SystemRoleId == r.ID
                                               where p.Active == true
                                               select new Permission
                                               {
                                                   PermissionId = p.Id,
                                                   Code = p.Code,
                                                   Name = p.Name

                                               }).ToList()
                            };

                return roles.ToList();
            }
        }
    }
}
