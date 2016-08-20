using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class RefreshTokenDao : IRefreshTokenDao
    {
        static RefreshTokenDao()
        {
            Mapper.CreateMap<tblIdentity_RefreshTokens, RefreshToken>();
            Mapper.CreateMap<RefreshToken, tblIdentity_RefreshTokens>();
        }

        public RefreshToken GetRefreshTokenById(string refreshTokenId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var token = context.tblIdentity_RefreshTokens.FirstOrDefault(c => c.ID == refreshTokenId) as tblIdentity_RefreshTokens;
                return Mapper.Map<tblIdentity_RefreshTokens, RefreshToken>(token);
            }
        }

        public int AddRefreshToken(RefreshToken token)
        {

            using (var context = new ApecIdentityEntities())
            {
                var existingToken = context.tblIdentity_RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientID == token.ClientId).SingleOrDefault();

                if (existingToken != null)
                {
                    var existingentity = Mapper.Map<tblIdentity_RefreshTokens, RefreshToken>(existingToken);

                    var result = RemoveRefreshToken(existingentity);
                }

                    var newentity = Mapper.Map<RefreshToken, tblIdentity_RefreshTokens>(token);

                    context.tblIdentity_RefreshTokens.Add(newentity);

                    return context.SaveChanges();
                

            }
        }

        public int RemoveRefreshToken(string refreshTokenId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entities = (from c in context.tblIdentity_RefreshTokens where c.ID == refreshTokenId select c).ToList();

                foreach (var e in entities)
                {
                    context.tblIdentity_RefreshTokens.Remove(e);
                    context.SaveChanges();
                }

            }

            return 1;
        }

        public int RemoveRefreshToken(RefreshToken refreshToken)
        {
            string id = refreshToken.Id;

            return RemoveRefreshToken(id);
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            using (var context = new ApecIdentityEntities())
            {
                var tokens = context.tblIdentity_RefreshTokens.ToList();

                List<RefreshToken> newtokens = new List<RefreshToken>();

                foreach (tblIdentity_RefreshTokens t in tokens)
                {
                    var nt = Mapper.Map<tblIdentity_RefreshTokens, RefreshToken>(t);

                    newtokens.Add(nt);
                }

                return newtokens;
            }
        }




    }
}
