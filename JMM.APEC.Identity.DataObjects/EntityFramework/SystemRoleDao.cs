using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class SystemRoleDao : ISystemRoleDao
    {
        static SystemRoleDao()
        {
            Mapper.CreateMap <tblIdentity_SystemRoles, SystemRole>();
            Mapper.CreateMap<SystemRole, tblIdentity_SystemRoles>();
        }

        public List<SystemRole> GetSystemRoles()
        {
            using (var context = new ApecIdentityEntities())
            {
                var roles = from r in context.tblIdentity_SystemRoles where r.Active == true where r.isDeleted == false select r;

                return roles.AsQueryable().Select(u =>
                    new SystemRole
                    {
                        ID = u.ID,
                        Code = u.Code,
                        Name = u.Name,
                        ServiceCode = u.ServiceCode
                    })
                    .ToList();
            }
        }

    }
}
