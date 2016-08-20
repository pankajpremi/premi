using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.WebAPI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Code
{
    public static class UserHelper
    {

        public static List<UserDto> ListUsers(List<User> UserList)
        {
            try
            {

                var Users = from t in UserList
                            select new UserDto()
                            {
                                CompanyName = t.Profile.CompanyName,
                                Email = t.Email,
                                FirstName = t.FirstName,
                                LastName = t.LastName,
                                Address1 = t.Address1,
                                Address2 = t.Address2,
                                City = t.City,
                                StateCode = t.State,
                                PostalCode = t.PostalCode,
                                CountryCode = t.Country,
                                TimeZoneCode = t.Profile.TimeZoneCode,
                                Gateways = t.GatewayCount,
                                StatusCode = t.StatusCode,
                                StatusId = t.StatusId,
                                StatusName = t.StatusName,
                                UserId = t.Id,
                                Username = t.UserName,
                                PhoneNumber = t.PhoneNumber,
                                LastLoginDate = t.LastLoginDate,
                                LastStatusUpdateDate = t.StatusUpdateDateTime  
                            };

                return Users.ToList();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public static UserDto MakeUserDto(User user)
        {
            var rUser = new UserDto
            {
                Address1 = user.Address1,
                Address2 = user.Address2,
                City = user.City,
                CompanyName = user.Profile.CompanyName,
                CountryCode = user.Country,
                PostalCode = user.PostalCode,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                StateCode = user.State,
                StatusCode = user.StatusCode,
                StatusId = user.StatusId,
                StatusName = user.StatusName,
                TimeZoneCode = user.Profile.TimeZoneCode,
                UserId = user.Id,
                Username = user.UserName,
                LastLoginDate = user.LastLoginDate,
                LastStatusUpdateDate = user.StatusUpdateDateTime
            };

            return rUser;
        }


    }
}