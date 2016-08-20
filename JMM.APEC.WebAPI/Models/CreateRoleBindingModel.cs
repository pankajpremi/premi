using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class CreateRoleBindingModel
    {
        [Required]
        [StringLength(256, ErrorMessageResourceType = typeof(Resources.LangResource), ErrorMessageResourceName = "PasswordTooShort")]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        public string ServiceId { get; set; }
    }

    public class UsersInRoleModel
    {
        public string Id { get; set; }
        public List<string> EnrolledUsers { get; set; }
        public List<string> RemovedUsers { get; set; }
    }
}