using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.DataAccess;
using JMM.APEC.WebAPI.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    public class ApplicationUserManager : UserManager<ApplicationIdentityUser, int>
    {
        public ApplicationUserManager(ApplicationUserStore<ApplicationIdentityUser, int> store) : base(store)
        {

        }

        public bool IsApproved(int userId)
        {
            var user = this.FindById(userId);

            return user.Approved;
        }

        public int GetCurrentPortalId(IOwinContext context, string portalDomain)
        {
            var database = context.Get<ApplicationDbContext>() as IdentityDatabase;
            var portalTable = new PortalTable(database);

            var portal = portalTable.GetByPortalId(portalDomain);

            if (portal !=null)
            {
                return portal.Id;
            }

            return 0;
        }

        public List<ApplicationGateway> GetUserGateways(IOwinContext context, int userId, int portalId)
        {
            var database = context.Get<ApplicationDbContext>() as IdentityDatabase;
            var userTable = new UserTable<ApplicationIdentityUser>(database);

            var gateways = userTable.GetUserGateways(userId, portalId);

            return gateways;
        }

        public List<ApplicationPortal> GetUserPortals(IOwinContext context, int userId, int portalId)
        {
            var database = context.Get<ApplicationDbContext>() as IdentityDatabase;
            var userTable = new UserTable<ApplicationIdentityUser>(database);

            var portals = userTable.GetUserPortals(userId, portalId);

            return portals;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var apiRequest = context.Request;
            var Headers = apiRequest.Headers;

            string portalDomain = null;
            int portalId = 1;

            if (Headers.ContainsKey("Portal_Id"))
            {
                portalDomain = Headers.GetValues("Portal_Id").First();
            }
            else
            {
                string defaultPortal = ConfigurationManager.AppSettings.Get("DefaultPortalUrl").ToString();
                portalDomain = defaultPortal;
            }

            var database = context.Get<ApplicationDbContext>() as IdentityDatabase;

            var appUserManager = new ApplicationUserManager(new ApplicationUserStore<ApplicationIdentityUser, int>(database, portalId));

            //Configure validation logic for usernames
            appUserManager.UserValidator = new CustomUserValidator<ApplicationIdentityUser, int>(appUserManager)
            {

            };

            //Configure password policy
            var passwordPolicyTable = new PasswordPolicyTable(database);
            PortalPasswordPolicy policy = passwordPolicyTable.GetByPortalId(portalDomain);

            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = policy.RequiredLength,
                RequireNonLetterOrDigit = policy.RequireNonLetterOrDigit,
                RequireDigit = policy.RequireDigit,
                RequireLowercase = policy.RequireLowercase,
                RequireUppercase = policy.RequireUppercase
            };

            bool UserLockoutEnabled = false;
            bool.TryParse(ConfigurationManager.AppSettings["UserLockoutEnabledByDefault"].ToString(), out UserLockoutEnabled);

            TimeSpan LockoutTimeSpan = TimeSpan.FromMinutes(Double.Parse(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));

            int MaxAttempts = 5;
            int.TryParse(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"].ToString(), out MaxAttempts);

            appUserManager.UserLockoutEnabledByDefault = UserLockoutEnabled;
            appUserManager.DefaultAccountLockoutTimeSpan = LockoutTimeSpan;
            appUserManager.MaxFailedAccessAttemptsBeforeLockout = MaxAttempts;

            int ValidationTokenSpan = 24;
            string timespan = ConfigurationManager.AppSettings.Get("ValidationTokenLifespan");
            int.TryParse(timespan, out ValidationTokenSpan);

            appUserManager.EmailService = new JMM.APEC.WebAPI.Services.IdentityEmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                var dataProtector = dataProtectionProvider.Create("ASP.NET Identity");

                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationIdentityUser, int>(dataProtector)
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(ValidationTokenSpan)
                } as IUserTokenProvider<ApplicationIdentityUser, int>;
            }

            return appUserManager;
        }

    }
}