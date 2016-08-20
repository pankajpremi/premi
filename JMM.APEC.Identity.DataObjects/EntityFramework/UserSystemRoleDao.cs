using AutoMapper;
using JMM.APEC.Core;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class UserSystemRoleDao : IUserSystemRoleDao
    {
        static UserSystemRoleDao()
        {
            Mapper.CreateMap<tblIdentity_UserSystemRoles, UserSystemRole>();
            Mapper.CreateMap<UserSystemRole, tblIdentity_UserSystemRoles>();
        }

        public List<UserSystemRole> FindByUserId(int portalId, int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var roles = from ur in context.tblIdentity_UserSystemRoles
                            join r in context.tblIdentity_SystemRoles on ur.RoleId equals r.ID
                            join ug in context.tblIdentity_UserGateways on new { ur.UserId, ur.GatewayId } equals new { ug.UserId, ug.GatewayId }
                            join g in context.tblIdentity_Gateways on ug.GatewayId equals g.ID
                            join p in context.tblIdentity_Permissions on ur.PermissionId equals p.Id
                            where ur.UserId == userId
                            where r.Active == true
                            select new UserSystemRole
                            {
                                RoleId = r.ID,
                                RoleName = r.Name,
                                RoleCode = r.Code,
                                PermissionId = p.Id,
                                PermissionName = p.Name,
                                PermissionCode = p.Code,
                                GatewayId = ur.GatewayId,
                                GatewayName = g.Name,
                                GatewayCode = g.Code,
                                UserId = ur.UserId,
                                PortalId = g.PortalID,
                                ServiceCode = r.ServiceCode
                            };

                return roles.ToList();
            }
        }

        public List<UserSystemRoleModel> FindByUserIdAndGateway(int portalId, int portalGatewayId, int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                List<UserSystemRoleModel> roles = new List<UserSystemRoleModel>();
                try
                { 

                var allroles = from sr in context.tblIdentity_SystemRoles
                            join gsr in context.tblIdentity_GatewaySystemRoles on sr.ID equals gsr.SystemRoleId
                            join g in context.tblIdentity_Gateways on gsr.GatewayId equals g.ID
                            join srp in context.tblIdentity_SystemRolePermissions on sr.ID equals srp.SystemRoleId
                            join p in context.tblIdentity_Permissions on srp.PermissionId equals p.Id
                            where sr.Active == true
                            where p.Active == true
                            where g.PortalGatewayID == portalGatewayId
                            select new UserSystemRole
                            {
                                RoleId = sr.ID,
                                RoleName = sr.Name,
                                RoleCode = sr.Code,
                                PermissionId = p.Id,
                                PermissionName = p.Name,
                                PermissionCode = p.Code,
                                GatewayId = g.ID,
                                GatewayName = g.Name,
                                GatewayCode = g.Code,
                                UserId = userId,
                                PortalId = (int)g.PortalGatewayID,
                                ServiceCode = sr.ServiceCode
                            };


                var userroles = from usr in context.tblIdentity_UserSystemRoles
                                join sr in context.tblIdentity_SystemRoles on usr.RoleId equals sr.ID
                                join g in context.tblIdentity_Gateways on usr.GatewayId equals g.ID
                                where usr.UserId == userId
                                where g.PortalGatewayID == portalGatewayId
                                select new UserSystemRole
                                {
                                    RoleId = sr.ID,
                                    RoleName = sr.Name,
                                    RoleCode = sr.Code,
                                    PermissionId = usr.PermissionId,
                                    GatewayId = usr.GatewayId,
                                    UserId = userId,
                                    ServiceCode = sr.ServiceCode
                                };

                    var distinctRoles = allroles.GroupBy(r => r.RoleId)
                           .Select(grp => grp.FirstOrDefault())
                           .ToList();

                    if (distinctRoles != null)
                    {

                        foreach (var d in distinctRoles)
                        {
                            var r = new UserSystemRoleModel();

                            var userR = (from ur in userroles where ur.RoleId == d.RoleId select ur).FirstOrDefault();

                            if (userR != null)
                            {
                                r.UserId = userR.UserId;
                                r.Selected = true;
                            }

                            r.SystemRoleId = d.RoleId;
                            r.SystemRoleName = d.RoleName;
                            r.SystemRoleCode = d.RoleCode;

                            var permissions = (from p in allroles
                                               where p.RoleId == d.RoleId
                                               select new UserPermissionModel
                                               {
                                                   PermissionId = p.PermissionId,
                                                   PermissionCode = p.PermissionCode,
                                                   PermissionName = p.PermissionName,
                                                   Selected = false,
                                                   UserId = 0
                                               }).ToList();

                            var pcntr = 0;

                            foreach (var pr in permissions)
                            {
                                var userP = (from ur in userroles where ur.RoleId == d.RoleId where ur.PermissionId == pr.PermissionId select ur).FirstOrDefault();

                                if (userP != null)
                                {
                                    permissions[pcntr].Selected = true;
                                    permissions[pcntr].UserId = userId;
                                }

                                pcntr++;
                            }

                            r.Permissions = permissions;

                            roles.Add(r);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return null;
                }

                return roles.ToList();
            }
        }

        public List<UserSystemRole> FindDistinctByUserId(int portalId, int userId)
        {
            var roles = FindByUserId(portalId, userId);

            var result = roles.GroupBy(role => role.RoleCode)
                   .Select(grp => grp.First())
                   .ToList();

            return result;
        }

        public List<UserSystemRole> FindDistinctByUserIdAndGateway(int portalId, int gatewayId, int userId)
        {
            //var roles = FindByUserIdAndGateway(portalId, gatewayId, userId);

            //var result = roles.GroupBy(role => role.RoleCode)
            //       .Select(grp => grp.First())
            //       .ToList();

            //return result;

            return null;
        }

        public int Delete(string userId)
        {

            return 1;
        }

        public int Delete(User user, SystemRole role)
        {

            return 1;
        }

        public int Insert(SystemRole role, string userId, int gatewayId, int portalId)
        {

            return 1;
        }


    }
}
