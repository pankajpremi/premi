using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class ForgotPasswordBindingModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "PortalIdRequired")]
        public string PortalId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}