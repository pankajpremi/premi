using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
   public class CreateFacilityBindingModel
    {
      
        [Required]
        public int GatewayId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string FacilityName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "FacilityAKA")]
        public string AKAName { get; set; }

        public int AddressId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public int TypeId { get; set; }

        [Required]
        [Display(Name = "Deleted?")]
        public bool IsDeleted { get; set; }

        [Required]
        public int AppChangeUserId { get; set; }
    }
}
