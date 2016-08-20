using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.BusinessObjects
{
    public class UserProfile : BusinessObject
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string TimeZoneCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAccessRequest { get; set; }
        public string AccessRightRequest { get; set; }
        public bool TermsAccepted { get; set; }
        public DateTime TermsAcceptedDateTime { get; set; }

    }
}
