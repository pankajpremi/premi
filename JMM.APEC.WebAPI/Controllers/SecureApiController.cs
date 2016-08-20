//using JMM.APEC.BusinessObjects;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.WebAPI.Helpers;
using JMM.APEC.WebAPI.Infrastructure;
using JMM.APEC.WebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using JMM.APEC.Core;

namespace JMM.APEC.WebAPI.Controllers
{
    public class SecureApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;

        public ApplicationSystemUser CurrentApiUser { get; set; }

        public SecureApiController()
        {
            var ci = GetUserIdentity;

            if (ci != null)
            {
                if (ci.IsAuthenticated == false)
                {
                    CurrentApiUser = null;
                    return;
                }

                CurrentApiUser = GetCurrentApiUser();
            }

        }

        protected ApplicationSystemUser GetCurrentApiUser()
        {
            var ci = GetUserIdentity;
            ApplicationSystemUser user = new ApplicationSystemUser();
            if (ci != null)
            {
                int userId = 0;
                int.TryParse(ci.GetUserId(), out userId);

                user.UserName = ci.Name;
                user.Id = userId;  

                ApplyClaims(ref user, ci.Claims);
            }

            return user;
        }

        private void ApplyClaims(ref ApplicationSystemUser user, IEnumerable<Claim> claims)
        {
            List<UserClaim> claimList = IdentityHelper.ReadUserClaims(claims, GetUserName);

            if (claimList.Count <= 0)
            {
                return;
            }

            var userPortal = (from p in claimList where p.ClaimType == "userportal" select p).FirstOrDefault();

            ApplicationUserPortal currentUserPortal = null;

            if (!String.IsNullOrEmpty(userPortal.ClaimValue))
            {
                currentUserPortal = JsonConvert.DeserializeObject<ApplicationUserPortal>(userPortal.ClaimValue);
                var valid = IdentityHelper.ValidatePortal(user.UserName, ref currentUserPortal);
                if (valid)
                {
                    user.IsValid = true;
                    user.PortalAllowed = true;
                }
            }

            var userGateways = (from g in claimList where g.ClaimType == "usergateway" select g).ToList();
            var userSystemRoles = (from r in claimList where r.ClaimType == "usersystemrole" select r).ToList();

            List<ApplicationUserGateway> currentUserGateways = new List<ApplicationUserGateway>();
            List<ApplicationUserSystemRole> currentUserSystemRoles = new List<ApplicationUserSystemRole>();

            var currentUserIdentityRoles = (claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)).ToList();

            user.IdentityRoles = currentUserIdentityRoles;

            var AdminRole = (from ar in user.IdentityRoles where ar.ToUpper() == "ADMIN" select ar).FirstOrDefault();
            var superAdminRole = (from sar in user.IdentityRoles where sar.ToUpper() == "SUPER ADMIN" select sar).FirstOrDefault();

            if (AdminRole != null)
            { user.IsAdmin = true; }

            if (superAdminRole != null)
            { user.IsSuperAdmin = true; }

            user.Portal = currentUserPortal;

            foreach (var gateway in userGateways)
            {
                if (!String.IsNullOrEmpty(gateway.ClaimValue))
                {
                    var currentGateway = JsonConvert.DeserializeObject<ApplicationUserGateway>(gateway.ClaimValue);

                    currentUserGateways.Add(currentGateway);
                }
            }

            user.Gateways = currentUserGateways;

            foreach (var role in userSystemRoles)
            {
                if (!String.IsNullOrEmpty(role.ClaimValue))
                {
                    var currentRole = JsonConvert.DeserializeObject<ApplicationUserSystemRole>(role.ClaimValue);

                    currentUserSystemRoles.Add(currentRole);
                }
            }

            user.SystemRoles = currentUserSystemRoles;

        }

        protected ClaimsIdentity GetUserIdentity
        {
            get
            {
                return User.Identity as ClaimsIdentity;
            }
        }

        protected string GetUserName
        {
            get
            {
                return GetUserIdentity.GetUserName();
            }
        }

        protected List<UserClaim> GetUserClaims
        {
            get
            {
                return GetUserIdentity.Claims.AsQueryable().Select(u =>
                                         new UserClaim
                                         {
                                             //Id = u.Id,
                                             UserName = User.Identity.Name,
                                             ClaimType = u.Type,
                                             ClaimValue = u.Value
                                         })
                                         .ToList();
            }
        }

        protected bool ClaimExists(string key)
        {
            return GetUserIdentity.HasClaim(x => x.Type == key);
        }

        protected UserClaim GetUserClaimByType(string key)
        {
            return GetUserClaims.Where(u => u.ClaimType == key).ToList().FirstOrDefault();
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected ModelFactory AppModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


    }
}
