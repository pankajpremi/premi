using JMM.APEC.ActionService;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.WebAPI.Areas.Admin.Models;
using JMM.APEC.WebAPI.Controllers;
using JMM.APEC.WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/users")]
    public class UserProfileController : SecureApiController
    {
        IIdentityService service { get; set; }

        public UserProfileController()
        {
            service = new IdentityService(CurrentApiUser);
        }

        [Route("{userid:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserById(int userid)
        {
            User user = null;
            List<User> UserList = null;

            user = service.GetUser(userid);

            if (User != null)
            {
                UserList = new List<User>();
                UserList.Add(user);

                var users = Code.UserHelper.ListUsers(UserList);

                return Ok(new MetadataWrapper<UserDto>(users));
            }

            return NotFound();
        }

        [Route("{userid:int}")]
        [Authorize]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateSelfUserProfile(int userid, [FromBody]UserProfileUpdateBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (userid < 0)
            {
                return BadRequest();
            }

            if (model == null)
            {
                return BadRequest();
            }

            //get existing user
            var user = service.GetUser(userid);

            if (user == null)
            {
                return BadRequest();
            }

            //update user profile
            user.Id = userid; // input id
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address1 = model.Address1;
            user.Address2 = model.Address2;
            user.City = model.City;
            user.Country = model.CountryCode;
            user.State = model.StateCode;
            user.PostalCode = model.PostalCode;
            user.PhoneNumber = model.PhoneNumber;
            user.Profile.CompanyName = model.CompanyName;
            user.Profile.TimeZoneCode = model.TimeZoneCode;

            service.UpdateUser(user);

            //update username/email
            if (user.UserName != model.Email)
            {
                var updateUser = await this.AppUserManager.FindByIdAsync(userid);

                if (updateUser != null)
                {
                    updateUser.UserName = model.Email;
                    updateUser.Email = model.Email;

                    IdentityResult result = await this.AppUserManager.UpdateAsync(updateUser);

                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }

                    //generate email verification code
                    string EmailConfirmationCode = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(updateUser.Id);

                    //specify callback url parameters
                    var callbackUrl = String.Format("?userId={0}&code={1}", user.Id, System.Web.HttpUtility.UrlEncode(EmailConfirmationCode));

                    //send email to user for validation
                    await this.AppUserManager.SendEmailAsync(user.Id, "EmailConfirm", callbackUrl.ToString());

                }
            }


            return Ok(Code.UserHelper.MakeUserDto(user));
        }

        [Route("{userid:int}/passwords")]
        [Authorize]
        [HttpPut]
        public async Task<IHttpActionResult> ResetSelfUserPassword(int userid, [FromBody]UserProfilePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (userid < 0)
            {
                return BadRequest();
            }

            if (model == null)
            {
                return BadRequest();
            }

            //get existing user
            var user = service.GetUser(userid);

            if (user == null)
            {
                return BadRequest();
            }

            string PasswordResetCode = await this.AppUserManager.GeneratePasswordResetTokenAsync(user.Id);

            IdentityResult result = await this.AppUserManager.ResetPasswordAsync(userid, PasswordResetCode, model.Password);

            if (result.Succeeded)
            {
                //re-get the user object 1
                var confirmuser = await this.AppUserManager.FindByIdAsync(userid);

                //send email to user to inform about password change
                await this.AppUserManager.SendEmailAsync(confirmuser.Id, "PasswordResetConfirm", null);

                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }

        }

    }
}
