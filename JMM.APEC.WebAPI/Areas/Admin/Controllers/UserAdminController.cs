using JMM.APEC.ActionService;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects;
using JMM.APEC.WebAPI.Areas.Admin.Models;
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
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Admin.Controllers
{
    [RoutePrefix("api/v1/admins/users")]
    [Authorize(Roles = "SUPER ADMIN,ADMIN,GATADM")]
    public class UserAdminController : SecureApiController
    {
        IIdentityService service { get; set; }
        IService portalService { get; set; }

        public UserAdminController()
        {
            service = new IdentityService(CurrentApiUser);
            portalService = new Service(CurrentApiUser);
        }

        private StatusDto MakeStatusDto(Status status)
        {
            var rStatus = new StatusDto
            {
                Code = status.Code,
                Name = status.Value,
                StatusId = status.Id
            };

            return rStatus;
        }

        private RoleDto MakeRoleDto(Role role)
        {
            var rRole = new RoleDto
            {
                RoleId = role.Id,
                Name = role.Name
            };

            return rRole;
        }

        private List<UserFacilityModelDto> ListFacilities(List<UserFacilityModel> FacilityList)
        {
            var Facilities = from f in FacilityList
                             select new UserFacilityModelDto()
                             {
                                 FacilityId = f.FacilityId,
                                 GatewayId = f.PortalGatewayId,
                                 Name = f.FacilityName,
                                 Selected = f.Selected,
                                 StateCode = f.StateCode,
                                 StateName = f.StateName,
                                 PortalId = f.PortalPortalId,
                                 UserId = f.UserId
                             };
            return Facilities.ToList();

        }

        private List<UserSystemRoleModelDto> ListSystemRoles(List<UserSystemRoleModel> RoleList)
        {
            var Roles = from t in RoleList
                        select new UserSystemRoleModelDto()
                        {
                            SystemRoleId = t.SystemRoleId,
                            Name = t.SystemRoleName,
                            Code = t.SystemRoleCode,
                            Selected = t.Selected,
                            UserId = t.UserId,
                            Permissions = (from p in t.Permissions select new UserPermissionModelDto
                            {
                                PermissionId = p.PermissionId,
                                Code = p.PermissionCode,
                                Name = p.PermissionName,
                                Selected = p.Selected,
                                UserId = p.UserId

                            }).ToList()

                        };
            return Roles.ToList();

        }

        [Route("")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserList([FromUri]UserManagementBindingModel model)
        {
            List<User> UserList = null;

            if (model != null)
            {
                UserList = service.GetUsers(model.Gateways, model.Statuses, model.SearchText, model.PageNum, model.PageSize, model.SortField, model.SortDirection);
            }
            else
            {
                UserList = service.GetUsers(null, null, null, null, null, null, null);
            }

            if (UserList != null & UserList.Count > 0)
            {
                var Users = Code.UserHelper.ListUsers(UserList);

                return Ok(new MetadataWrapper<UserDto>(Users));
            }

            return NotFound();

        }

        [Route("{userid:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserById(int userid)
        {
            User User = null;
            List<User> UserList = null;

            User = service.GetUser(userid);

            if (User != null)
            {
                UserList = new List<User>();
                UserList.Add(User);

                var users = Code.UserHelper.ListUsers(UserList);

                return Ok(new MetadataWrapper<UserDto>(users));
            }

            return NotFound();
        }

        [Route("{userid:int}")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult UpdateUser(int userid, [FromBody]UserManagementUpdateBindingModel model)
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

            model.CompanyName = user.Profile.CompanyName;

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

            return Ok(Code.UserHelper.MakeUserDto(user));
        }

        [Route("{userid:int}")]
        [Authorize]
        [HttpDelete]
        public IHttpActionResult DeleteUser(int userid)
        {
            service.DeleteUser(userid);

            return Ok();
        }

        [Route("{userid:int}/statuses")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserStatus(int userid)
        {
            Status status = null;
            status = service.GetStatusForUser(userid);

            if (status != null)
            {
                return Ok(MakeStatusDto(status));
            }

            return NotFound();
        }

        [Route("{userid:int}/statuses")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult UpdateUserStatus(int userid, [FromBody]UserManagementUpdateStatusModel model)
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

            return Ok(Code.UserHelper.MakeUserDto(user));
        }

        [Route("{userid:int}/roles")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserRole(int userid)
        {
            Role role = null;
            role = service.GetRoleForUser(userid);

            if (role != null)
            {
                return Ok(MakeRoleDto(role));
            }

            return NotFound();
        }

        [Route("{userid:int}/systemroles/{gatewayid:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserSystemRoles(int userid, int gatewayid)
        {
            List<UserSystemRoleModel> RoleList = null;
            RoleList = service.GetSystemRolesForUser(CurrentApiUser.Portal.PortalId, gatewayid, userid);

            if (RoleList != null & RoleList.Count > 0)
            {
                var Roles = ListSystemRoles(RoleList);

                return Ok(new MetadataWrapper<UserSystemRoleModelDto>(Roles));
            }

            return NotFound();
        }

        [Route("{userid:int}/systemroles/{gatewayid:int}")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult UpdateUserSystemRoles(int userid, int gatewayid, [FromBody]UserSystemRolesUpdateModel model)
        {
            //List<UserSystemRoleModel> RoleList = null;
            //RoleList = service.GetSystemRolesForUser(CurrentApiUser.Portal.PortalId, gatewayid, userid);

            //if (RoleList != null & RoleList.Count > 0)
            //{
            //    var Roles = ListSystemRoles(RoleList);

            //    return Ok(new MetadataWrapper<UserSystemRoleModelDto>(Roles));
            //}

            return Ok();
        }

        [Route("{userid:int}/facilities/{gatewayid:int}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetUserFacilities(int userid, int gatewayid)
        {
            ///var facilities = portalService.GetFacilities(gatewayid, null, null, null, null, null, null, null, null, null, null);

            List<UserFacilityModel> FacilityList = null;
            FacilityList = service.GetFacilitiesForUser(CurrentApiUser.Portal.PortalId, gatewayid, userid);

            if (FacilityList != null & FacilityList.Count > 0)
            {
                var Facilities = ListFacilities(FacilityList);

                return Ok(new MetadataWrapper<UserFacilityModelDto>(Facilities));
            }

            return NotFound();
        }

        [Route("{userid:int}/facilities/{gatewayid:int}")]
        [Authorize]
        [HttpPut]
        public IHttpActionResult UpdateUserFacilities(int userid, int gatewayid, [FromBody]UserFacilitiesUpdateModel model)
        {
            //List<UserFacilityModel> FacilityList = null;
            //FacilityList = service.GetFacilitiesForUser(CurrentApiUser.Portal.PortalId, gatewayid, userid);

            //if (FacilityList != null & FacilityList.Count > 0)
            //{
            //    var Facilities = ListFacilities(FacilityList);

            //    return Ok(new MetadataWrapper<UserFacilityModelDto>(Facilities));
            //}

            return Ok();
        }

        [Route("{userid:int}/passwords")]
        [Authorize]
        [HttpPut]
        public async Task<IHttpActionResult> AdminResetPassword(int userid)
        {
            if (userid == 0)
            {
                ModelState.AddModelError("invalidRequest", "User Id is required.");
                return BadRequest(ModelState);
            }

            var user = await this.AppUserManager.FindByIdAsync(userid);

            if (user == null || !(await this.AppUserManager.IsEmailConfirmedAsync(user.Id)))
            {
                ModelState.AddModelError("invalidUser", "Invalid user account.");
                return BadRequest(ModelState);
            }

            if (!user.Approved || user.LockoutEnabled)
            {
                ModelState.AddModelError("lockedUser", "User account is locked out.");
                return BadRequest(ModelState);
            }

            //lock the user out
            IdentityResult result = await this.AppUserManager.SetLockoutEnabledAsync(user.Id, true);
            if (result.Succeeded)
            {
                user = await this.AppUserManager.FindByIdAsync(userid);
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

    }
}
