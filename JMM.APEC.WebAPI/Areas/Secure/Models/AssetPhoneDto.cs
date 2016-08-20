using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetPhoneDto
    {
        public int PhoneId { get; set; }
        public string Number { get; set; }
        public int TypeId { get; set; }
        public int AppChangeUserId { get; set; }
    }
}