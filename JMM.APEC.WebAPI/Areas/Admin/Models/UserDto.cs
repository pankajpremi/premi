using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string TimeZoneCode { get; set; }
        public int Gateways { get; set; }
        public string CompanyName { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastStatusUpdateDate { get; set; }
    }
}