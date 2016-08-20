using JMM.APEC.ActionService;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.DataAccess;
using JMM.APEC.WebAPI.Infrastructure;
using JMM.APEC.WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Public.Controllers
{
    [RoutePrefix("api/v1/accounts")]
    public class AccountController : BaseApiController
    {
        private IdentityDatabase database = null;
        private PortalTable portalTable;
        private IMessageService messageService;

        public AccountController()
        {
            database = new IdentityDatabase();
            portalTable = new PortalTable(database);
        }

        [Route("")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> RequestAccess(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationPortal appPortal = portalTable.GetByPortalId(createUserModel.PortalId);
                List<ApplicationPortal> appPortals = new List<ApplicationPortal>();

                if (appPortal == null || appPortal.Active == false)
                {
                    ModelState.AddModelError("invalidPortal", "The supplied Portal information is invalid.");
                    return BadRequest(ModelState);
                }

                appPortals.Add(appPortal);

                var user = new ApplicationIdentityUser()
                {
                    UserName = createUserModel.Username,
                    Email = createUserModel.Email,
                    FirstName = createUserModel.FirstName,
                    LastName = createUserModel.LastName,
                    Address1 = createUserModel.Address1,
                    Address2 = createUserModel.Address2,
                    CountryCode = createUserModel.CountryCode,
                    StateCode = createUserModel.StateCode,
                    City = createUserModel.City,
                    PostalCode = createUserModel.PostalCode,
                    PhoneNumber = createUserModel.Phone,
                    LockoutEnabled = false,
                    Approved = false,
                    SignUpDate = DateTime.UtcNow,
                    Portals = appPortals,
                    StatusCode = "REGTRD",
                    StatusUpdateDateTime = DateTime.UtcNow,
                    Profile = new IdentityUserProfile()
                    {
                        AccessRightRequest = createUserModel.AccessRequest,
                        CompanyAccessRequest = createUserModel.CompanyRequest,
                        CompanyName = createUserModel.YourCompany,
                        TimeZoneCode = createUserModel.TimeZoneCode,
                        TermsAccepted = true,
                        TermsAcceptedDateTime = DateTime.UtcNow,
                        UserId = 0
                    }
                };

                //create user account
                IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }

                //send email to administrators
                messageService = new MessageService(user.Portals.FirstOrDefault().Id, null);
                await messageService.NotifyEmailAdmins("USRRQT", user);

                //update user status 
                user.StatusCode = "PNDVER";
                user.StatusUpdateDateTime = DateTime.UtcNow;
                var result = await this.AppUserManager.UpdateAsync(user);

                //generate email verification code
                string EmailConfirmationCode = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                //specify callback url parameters
                var callbackUrl = String.Format("?userId={0}&code={1}", user.Id, System.Web.HttpUtility.UrlEncode(EmailConfirmationCode));

                //send email to user for validation
                await this.AppUserManager.SendEmailAsync(user.Id, "EmailConfirm", callbackUrl.ToString());

                Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

                return Created(locationHeader, AppModelFactory.Create(user));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [Route("users/{id:int}", Name = "GetUserById")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var user = await this.AppUserManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(this.AppModelFactory.Create(user));
            }

            return NotFound();
        }

        [Route("emails", Name = "ConfirmEmailRoute")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(int userId = 0, string code = "")
        {
            if (userId == 0 || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("invalidRequest", "UserId and Code are required.");
                return BadRequest(ModelState);
            }

            //code = System.Web.HttpUtility.UrlDecode(code);

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                var user = await this.AppUserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    //send email to administrators
                    messageService = new MessageService(user.Portals.FirstOrDefault().Id, null);
                    await messageService.NotifyEmailAdmins("USRVLD", user);

                }

                return Ok();

            }
            else
            {
                var list = (from e in result.Errors select e).ToList();
                if (list.Count > 0)
                {
                    var errors1 = string.Format("{0} - {1} - {2}", result.Succeeded.ToString(), list[0].ToString(), code);
                    Logging.LogWriter.Log.LogMessage(userId.ToString(), errors1);
                }

                return GetErrorResult(result);
            }

        }

        [Route("passwords", Name = "ForgotPassword")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordBindingModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationPortal appPortal = portalTable.GetByPortalId(forgotPasswordModel.PortalId);
            if (appPortal == null || appPortal.Active == false)
            {
                ModelState.AddModelError("invalidRequest", "Invalid request.");
                return BadRequest(ModelState);
            }

            var user = await this.AppUserManager.FindByNameAsync(forgotPasswordModel.Email);
            if (user == null || !(await this.AppUserManager.IsEmailConfirmedAsync(user.Id)))
            {
                ModelState.AddModelError("invalidUser", "Invalid user account.");
                return BadRequest(ModelState);
            }

            if (!user.Approved || user.LockoutEnabled)
            {
                ModelState.AddModelError("invalidUser", "Invalid user account.");
                return BadRequest(ModelState);
            }

            //lock the user out
            IdentityResult result = await this.AppUserManager.SetLockoutEnabledAsync(user.Id, true);
            if(result.Succeeded)
            {
                user = await this.AppUserManager.FindByEmailAsync(forgotPasswordModel.Email);
            }
            else
            {
                return InternalServerError(new Exception("Unable to reset password."));
            }

            string PasswordResetCode = await this.AppUserManager.GeneratePasswordResetTokenAsync(user.Id);

            //update user status 
            user.StatusCode = "PSSRST";
            user.StatusUpdateDateTime = DateTime.UtcNow;
            var statusResult = await this.AppUserManager.UpdateAsync(user);

            //specify callback url parameters
            var callbackUrl = String.Format("?userId={0}&code={1}", user.Id, System.Web.HttpUtility.UrlEncode(PasswordResetCode));

            //send email to user for validation
            await this.AppUserManager.SendEmailAsync(user.Id, "PasswordReset", callbackUrl.ToString());

            return Ok();
        }

        [Route("passwords/{id:int}", Name = "ResetPassword")]
        [AllowAnonymous]
        [HttpPut]
        public async Task<IHttpActionResult> ResetPassword(int id, [FromBody]ResetPasswordBindingModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == 0 || string.IsNullOrWhiteSpace(resetPasswordModel.Code))
            {
                ModelState.AddModelError("invalidRequest", "User Id and Code are required.");
                return BadRequest(ModelState);
            }

            var user = await this.AppUserManager.FindByIdAsync(id);
            if (user == null || !(await this.AppUserManager.IsEmailConfirmedAsync(user.Id)) || !(this.AppUserManager.IsApproved(user.Id)))
            {
                ModelState.AddModelError("invalidRequest", "Password reset request is invalid.");
                return BadRequest(ModelState);
            }

            var code = System.Web.HttpUtility.UrlDecode(resetPasswordModel.Code);
            //var code = resetPasswordModel.Code;

            IdentityResult result = await this.AppUserManager.ResetPasswordAsync(id, code, resetPasswordModel.Password);

            if (result.Succeeded)
            {
                //re-get the user object 1
                user = await this.AppUserManager.FindByIdAsync(id);

                //unlock the user account
                result = await this.AppUserManager.SetLockoutEnabledAsync(user.Id, false);

                //re-get the user object 2
                user = await this.AppUserManager.FindByIdAsync(id);

                //update user status 
                user.StatusCode = "APPRVD";
                user.StatusUpdateDateTime = DateTime.UtcNow;
                var statusResult = await this.AppUserManager.UpdateAsync(user);

                //send email to user to inform about password change
                await this.AppUserManager.SendEmailAsync(user.Id, "PasswordResetConfirm", null);

                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }

        }

        [Route("passwords/policies", Name = "PasswordPolicy")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> VerifyPasswordPolicy(string password = "")
        {
            var rules = await this.AppUserManager.PasswordValidator.ValidateAsync(password);

            var list = rules.Errors.First().Split('.');

            return Ok(new MetadataWrapper<string>(list));
        }

    }
}
