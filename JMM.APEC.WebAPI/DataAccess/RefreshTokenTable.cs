using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JMM.APEC.WebAPI.DataAccess
{
    public class RefreshTokenTable
    {
        private IdentityDatabase _database;
        private IRefreshTokenDao tokenDao;

        public RefreshTokenTable(IdentityDatabase database)
        {
            _database = database;
            IDaoFactory factory = database.GetFactory();

            tokenDao = factory.RefreshTokenDao;
        }

        public ApplicationRefreshToken GetRefreshTokenById(string refreshTokenId)
        {
            RefreshToken oToken = null;
            oToken = tokenDao.GetRefreshTokenById(refreshTokenId);

            ApplicationRefreshToken tn = null;

            if (oToken != null)
            {
                tn = new ApplicationRefreshToken();
                tn.ClientId = oToken.ClientId;
                tn.ExpiresUtc = oToken.ExpiresUtc;
                tn.Id = oToken.Id;
                tn.IssuedUtc = oToken.IssuedUtc;
                tn.ProtectedTicket = oToken.ProtectedTicket;
                tn.Subject = oToken.Subject;
            }

            return tn;
        }

        public async Task<bool> AddRefreshToken(ApplicationRefreshToken token)
        {

            RefreshToken oRefreshToken = new RefreshToken();
            oRefreshToken.Id = token.Id;
            oRefreshToken.ExpiresUtc = token.ExpiresUtc;
            oRefreshToken.ClientId = token.ClientId;
            oRefreshToken.IssuedUtc = token.IssuedUtc;
            oRefreshToken.ProtectedTicket = token.ProtectedTicket;
            oRefreshToken.Subject = token.Subject;

            var result = tokenDao.AddRefreshToken(oRefreshToken);

            return true;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var result = tokenDao.RemoveRefreshToken(refreshTokenId);

            return true;
        }

        public async Task<bool> RemoveRefreshToken(ApplicationRefreshToken token)
        {
            var result = tokenDao.RemoveRefreshToken(token.Id);

            return true;
        }

        public List<ApplicationRefreshToken> GetAllRefreshTokens()
        {
            return null;
        }

    }
}