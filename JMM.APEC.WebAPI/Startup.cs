using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.DataAccess;
using JMM.APEC.WebAPI.Infrastructure;
using JMM.APEC.WebAPI.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using JMM.APEC.WebAPI.Logging;
using System.Web.Http.ExceptionHandling;
using JMM.APEC.WebAPI.ExceptionHandling;
using JMM.APEC.WebAPI.Filters;
using Microsoft.Owin.Security.DataProtection;

[assembly: OwinStartup(typeof(JMM.APEC.WebAPI.Startup))]
namespace JMM.APEC.WebAPI
{
    public class Startup
    {
        //public static IDataProtectionProvider DataProtectionProvider { get; set; }

        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration httpConfig = new HttpConfiguration();

            //DataProtectionProvider = app.GetDataProtectionProvider();

            ConfigureOAuthTokenGeneration(app);

            ConfigureOAuthTokenConsumption(app);

            ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);

        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            int TokenSpan = 60;
            string provider = ConfigurationManager.AppSettings.Get("IdentityDataProvider");
            string timespan = ConfigurationManager.AppSettings.Get("TokenLifespan");
            int.TryParse(timespan, out TokenSpan);

            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            var configIssuer = ConfigurationManager.AppSettings["ApiUrl"].ToString();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(TokenSpan),
                Provider = new CustomOAuthProvider(),
                RefreshTokenProvider = new CustomRefreshTokenProvider(),
                AccessTokenFormat = new CustomJwtFormat(configIssuer)
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var configIssuer = ConfigurationManager.AppSettings["ApiUrl"].ToString();

            var issuer = configIssuer;

            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            SetupSemanticLogBlock.SetupSemanticLoggingApplicationBlock();
            
            //filter for measuring time taken to execute action method
            config.Filters.Add(new TimingActionFilter());
            config.Filters.Add(new LogActionFilter());

            config.MessageHandlers.Add(new JMM.APEC.WebAPI.Logging.ApiLogHandler());
            config.MessageHandlers.Add(new LanguageMessageHandler());

            //global exception logger & handler           
            config.Services.Replace(typeof(IExceptionHandler), new MyGlobalExceptionHandler());
            config.Services.Add(typeof(IExceptionLogger), new SlabLogExceptionLogger());

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }


        
    }
}