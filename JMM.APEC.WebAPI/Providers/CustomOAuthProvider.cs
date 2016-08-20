using JMM.APEC.WebAPI.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using JMM.APEC.WebAPI.Authorization;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.DataAccess;
using System.Configuration;

namespace JMM.APEC.WebAPI.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            ApplicationClient client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context
                //if you want to force sending clientid/secrets once obtain access tokens.
                context.Validated();
                context.SetError("clientMissing", "Client infromation shold be provided.");
                return Task.FromResult<object>(null);
            }

            IdentityDatabase database = new IdentityDatabase();
            ClientTable clientTable;
            clientTable = new ClientTable(database);

            client = clientTable.GetClientById(context.ClientId);

            if (client == null)
            {
                context.SetError("clientUnknown", string.Format("Client '{0}' is not registered in this system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == Models.ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("clientAuth", "Client '{0}' secret code should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("clientInvalid", "Client '{0}' access code is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("clientInactive", "Client '{0}' is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifetime", client.RefreshTokenLifetime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var currentHost = context.Request.Headers.Get("Host");
            Logging.LogWriter.Log.LogMessage("Admin", String.Format("Host: {0}", currentHost));

            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationIdentityUser user = await userManager.FindAsync(context.UserName, context.Password);

             bool success = true;
             var remoteIpAddresss = context.Request.RemoteIpAddress;

            if (user == null)
            {
                success = false;
                context.SetError("authIncorrect", "The username or password is incorrect.");

                var failedUser = await userManager.FindByNameAsync(context.UserName);

                if (failedUser != null)
                {
                    await userManager.AccessFailedAsync(failedUser.Id);

                    if (await userManager.IsLockedOutAsync(failedUser.Id))
                    {
                        context.SetError("unauthorizedLocked", "User is locked out.");
                    }
                }

                Logging.LogWriter.Log.LoginAttemptsLog(context.UserName, success, remoteIpAddresss, DateTime.Now);
                return;
            }

            if (!user.EmailConfirmed)
            {
                success = false;
                context.SetError("unauthorizedEmail", "User did not confirm email.");
                Logging.LogWriter.Log.LoginAttemptsLog(context.UserName, success, remoteIpAddresss, DateTime.Now);
                return;
            }

            if (!user.Approved)
            {
                success = false;
                context.SetError("unauthorizedApproved", "User is not approved.");
                Logging.LogWriter.Log.LoginAttemptsLog(context.UserName, success, remoteIpAddresss, DateTime.Now);
                return;
            }

            if (user.LockoutEnabled)
            {
                if (user.LockoutEndDateUtc < DateTime.UtcNow)
                {
                    await userManager.SetLockoutEnabledAsync(user.Id, false);
                    return;
                }

                success = false;
                context.SetError("unauthorizedLocked", "User is locked out.");
                Logging.LogWriter.Log.LoginAttemptsLog(context.UserName, success, remoteIpAddresss, DateTime.Now);
                return;
            }

            //logging user login attempts to web api - 8/19/2015
            Logging.LogWriter.Log.LoginAttemptsLog(context.UserName, success, remoteIpAddresss, DateTime.Now);

            IFormCollection userdata = await context.Request.ReadFormAsync();
            string portalDomain = userdata["portal_id"];
            int currentPortalId = 0;

            //get portalId
            currentPortalId = userManager.GetCurrentPortalId(context.OwinContext, portalDomain);

            if (currentPortalId <= 0)
            {
                context.SetError("unauthorizedPortal", "This user is not allowed to access this portal.");
                return;
            }
            
            //get user portal and gateway assignments
            user.Portals = userManager.GetUserPortals(context.OwinContext, user.Id, currentPortalId);
            user.Gateways = userManager.GetUserGateways(context.OwinContext, user.Id, currentPortalId);

            if (!user.VerifyPortalAccess(portalDomain, ref currentPortalId))
            {
                context.SetError("unauthorizedPortal", "This user is not allowed to access this portal.");
                return;
            }

            user.CurrentPortalId = currentPortalId;

            //if log in is successfull update the login attempts to 0
            user.AccessFailedCount = 0;
            await userManager.UpdateAsync(user);

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");

            //get user claims
            oAuthIdentity.AddClaims(ExtendedClaimProvider.GetClaims(user));

            //get user roles
            oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity, currentPortalId));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {"as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId},
                {"as:portal_id", currentPortalId.ToString() },
                {"username", context.UserName}
            });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);

            context.Validated(ticket);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            //Change the auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("refreshtoken", "true"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }


    }
}