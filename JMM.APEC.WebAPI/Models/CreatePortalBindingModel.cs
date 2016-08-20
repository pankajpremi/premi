using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
    public class CreatePortalBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Url]
        [DataType(DataType.Url)]
        [Display(Name = "DomainUrl")]
        public string Url { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Required]
        public int AppChangeUserId { get; set; }
    }
}
