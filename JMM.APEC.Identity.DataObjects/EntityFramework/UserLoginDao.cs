using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class UserLoginDao : IUserLoginDao
    {
        static UserLoginDao()
        {
            Mapper.CreateMap<AspNetUserLogin, UserLogin>();
            Mapper.CreateMap<UserLogin, AspNetUserLogin>();
        }

        public int Delete(User user, UserLogin login)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetUserLogins.SingleOrDefault(m => m.UserId == user.Id && m.LoginProvider == login.LoginProvider && m.ProviderKey == login.ProviderKey);
                context.AspNetUserLogins.Remove(entity);
                context.SaveChanges();
            }

            return 1;
        }

        public int Delete(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entities = (from c in context.AspNetUserLogins where c.UserId == userId select c).ToList();

                foreach (var e in entities)
                {
                    context.AspNetUserLogins.Remove(e);
                    context.SaveChanges();
                }

            }

            return 1;
        }

        public int Insert(User user, UserLogin login)
        {

            using (var context = new ApecIdentityEntities())
            {
                login.UserId = user.Id;

                var entity = Mapper.Map<UserLogin, AspNetUserLogin>(login);

                context.AspNetUserLogins.Add(entity);
                context.SaveChanges();

                // update business object with new id
                login.UserId = entity.UserId;
            }

            return 1;

        }

        public int FindUserIdByLogin(UserLogin userLogin)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetUserLogins.SingleOrDefault(m => m.LoginProvider == userLogin.LoginProvider && m.ProviderKey == userLogin.ProviderKey);
                return entity.UserId;
            }
        }

        public List<UserLogin> FindByUserId(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var logins = from u in context.AspNetUserLogins where u.UserId == userId select u;

                return logins.AsQueryable().Select(u =>
                    new UserLogin
                    {
                        UserId = u.UserId,
                        ProviderKey = u.ProviderKey,
                        LoginProvider = u.LoginProvider

                    })
                    .ToList();
            }
        }

    }
}
