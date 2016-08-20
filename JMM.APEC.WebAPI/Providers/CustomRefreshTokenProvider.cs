using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Authorization;
using JMM.APEC.WebAPI.DataAccess;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JMM.APEC.WebAPI.Providers
{
    public class CustomRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            IdentityDatabase database = new IdentityDatabase();

            RefreshTokenTable refreshTokentable;
            refreshTokentable = new RefreshTokenTable(database);

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifetime");

            var token = new ApplicationRefreshToken()
            {
                Id = Helper.GetHash(refreshTokenId),
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))

            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await refreshTokentable.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }

        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            IdentityDatabase database = new IdentityDatabase();

            RefreshTokenTable refreshTokenTable;
            refreshTokenTable = new RefreshTokenTable(database);

            var refreshToken = refreshTokenTable.GetRefreshTokenById(hashedTokenId);

            if (refreshToken != null)
            {
                //get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);

                var result = await refreshTokenTable.RemoveRefreshToken(hashedTokenId);
            }

        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}