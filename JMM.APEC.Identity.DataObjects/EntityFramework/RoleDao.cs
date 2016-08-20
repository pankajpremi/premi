using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class RoleDao : IRoleDao
    {
        static RoleDao()
        {
            Mapper.CreateMap<AspNetRole, Role>();
            Mapper.CreateMap<Role, AspNetRole>();
        }

        public int Delete(int roleId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entities = (from c in context.AspNetRoles where c.Id == roleId select c).ToList();

                foreach (var e in entities)
                {
                    context.AspNetRoles.Remove(e);
                    context.SaveChanges();
                }

            }

            return 1;
        }

        public int Insert(Role role)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = Mapper.Map<Role, AspNetRole>(role);

                context.AspNetRoles.Add(entity);
                context.SaveChanges();

                // update business object with new id
                role.Id = entity.Id;
            }

            return 1;
        }

        public List<Role> GetRoles()
        {
            using (var context = new ApecIdentityEntities())
            {
                var roles = from r in context.AspNetRoles select r;

                return roles.AsQueryable().Select(u =>
                    new Role
                    {
                        Id = u.Id,
                        Name = u.Name
                    })
                    .ToList();
            }
        }

        public string GetRoleName(int roleId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetRoles.SingleOrDefault(m => m.Id == roleId);
                return entity.Name;
            }
        }

        public int GetRoleId(string roleName)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetRoles.SingleOrDefault(m => m.Name == roleName);
                return entity.Id;
            }
        }

        public Role GetRoleById(int roleId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var role = context.AspNetRoles.FirstOrDefault(c => c.Id == roleId) as AspNetRole;
                return Mapper.Map<AspNetRole, Role>(role);
            }
        }

        public Role GetRoleByUserId(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = (from ur in context.AspNetUserRoles where ur.UserId == userId select ur.AspNetRole).FirstOrDefault();
                return Mapper.Map<AspNetRole, Role>(entity);
            }
        }

        public Role GetRoleByName(string roleName)
        {
            using (var context = new ApecIdentityEntities())
            {
                var role = context.AspNetRoles.FirstOrDefault(c => c.Name == roleName) as AspNetRole;
                return Mapper.Map<AspNetRole, Role>(role);
            }
        }

        public int Update(Role role)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetRoles.SingleOrDefault(m => m.Id == role.Id);

                entity.Name = role.Name;
                entity.Description = "Role";

                //context.Members.Attach(entity); 
                context.SaveChanges();

            }

            return 1;        
        }


    }
}
