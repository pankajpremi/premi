using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class PermissionDao : IPermissionDao
    {
        static PermissionDao()
        {
            Mapper.CreateMap<tblIdentity_Permissions, Permission>();
            Mapper.CreateMap<Permission, tblIdentity_Permissions>();
        }

        public List<Permission> GetPermissions()
        {
            using (var context = new ApecIdentityEntities())
            {
                var permissions = from r in context.tblIdentity_Permissions where r.Active == true select r;

                return permissions.AsQueryable().Select(u =>
                    new Permission
                    {
                        PermissionId = u.Id,
                        Name = u.Name,
                        Code = u.Code

                    })
                    .ToList();
            }
        }
    }
}
