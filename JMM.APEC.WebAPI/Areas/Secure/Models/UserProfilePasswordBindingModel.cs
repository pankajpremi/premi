using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class UserProfilePasswordBindingModel
    {
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}