using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class UserRoleDao : IUserRoleDao
    {
        static UserRoleDao()
        {
            Mapper.CreateMap<AspNetUserRole, UserRole>();
            Mapper.CreateMap<UserRole, AspNetUserRole>();
        }

        public List<string> FindByUserId(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var roles = (from u in context.AspNetUserRoles where u.UserId == userId select u);

                int[] roleIds = roles.Select(d => d.RoleId).ToArray();

                var roleNames = (from r in context.AspNetRoles where roleIds.Contains(r.Id) select r.Name).ToList();

                return roleNames;
            }
        }

        public int Delete(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entities = (from c in context.AspNetUserRoles where c.UserId == userId select c).ToList();

                foreach (var e in entities)
                {
                    context.AspNetUserRoles.Remove(e);
                    context.SaveChanges();
                }
            }

            return 1;
        }

        public int Insert(User user, int roleId)
        {
            using (var context = new ApecIdentityEntities())
            {

                var entity = new UserRole();
                entity.UserId = user.Id;
                entity.RoleId = roleId;

                var newentity = Mapper.Map<UserRole, AspNetUserRole>(entity);

                context.AspNetUserRoles.Add(newentity);
                context.SaveChanges();
            }

            return 1;
        }


    }
}
