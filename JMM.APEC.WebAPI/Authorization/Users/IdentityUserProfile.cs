using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Infrastructure
{
    public class IdentityUserProfile
    {
        public IdentityUserProfile()
        {
            Id = 0;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string TimeZoneCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAccessRequest { get; set; }
        public string AccessRightRequest { get; set; }
        public bool TermsAccepted { get; set; }
        public DateTime TermsAcceptedDateTime { get; set; }
    }
}