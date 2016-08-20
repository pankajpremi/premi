using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using JMM.APEC.WebAPI.Resources;


namespace JMM.APEC.WebAPI.Models
{
    public class CreateUserBindingModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "PortalIdRequired")]
        public string PortalId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "FirstNameRequired")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "LastNameRequired")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]
        public string CountryCode { get; set; }

        [Display(Name = "State")]
        public string StateCode { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.LangResource), AllowEmptyStrings = false, ErrorMessageResourceName = "UserNameRequired")]
        [Display(Name = "Username")]
        public string Username { get; set; }

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

        public string TimeZoneCode { get; set; }

        public string YourCompany { get; set; }

        public string CompanyRequest { get; set; }

        public string AccessRequest { get; set; }
    }

    public class ChangePasswordBidingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.LangResource), ErrorMessageResourceName = "PasswordTooShort")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.LangResource), ErrorMessageResourceName = "NewPasswordMatch")]
        public string ConfirmPassword { get; set; }
    }

}