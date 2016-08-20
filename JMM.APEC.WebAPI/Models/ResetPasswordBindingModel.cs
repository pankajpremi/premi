using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class ResetPasswordBindingModel
    {
        public string Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.LangResource), ErrorMessageResourceName = "PasswordTooShort")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.LangResource), ErrorMessageResourceName = "PasswordMatch")]
        public string ConfirmPassword { get; set; }
    }
}