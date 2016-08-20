using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class UserClaimDao : IUserClaimDao
    {
        static UserClaimDao()
        {
            Mapper.CreateMap<AspNetUserClaim, UserClaim>();
            Mapper.CreateMap<UserClaim, AspNetUserClaim>();
        }

        public List<UserClaim> FindByUserId(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var claims = from u in context.AspNetUserClaims where u.UserId == userId select u;

                return claims.AsQueryable().Select(u =>
                    new UserClaim
                    {
                        Id = u.Id,
                        UserId = u.UserId,
                        ClaimType = u.ClaimType,
                        ClaimValue = u.ClaimValue
                    })
                    .ToList();
            }
        }

        public int Delete(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entities = (from c in context.AspNetUserClaims where c.UserId == userId select c).ToList();

                foreach (var e in entities)
                {
                    context.AspNetUserClaims.Remove(e);
                    context.SaveChanges();
                }

            }

            return 1;
        }

        public int Delete(User user, UserClaim claim)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetUserClaims.SingleOrDefault(m => m.UserId == user.Id && m.ClaimType == claim.ClaimType && m.ClaimValue == claim.ClaimValue);
                context.AspNetUserClaims.Remove(entity);
                context.SaveChanges();
            }

            return 1;
        }

        public int Insert(UserClaim userClaim, int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                userClaim.UserId = userId;
                var entity = Mapper.Map<UserClaim, AspNetUserClaim>(userClaim);

                context.AspNetUserClaims.Add(entity);
                context.SaveChanges();

                // update business object with new id
                userClaim.Id = entity.Id;
            }

            return 1;
        }

    }
}
