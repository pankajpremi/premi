using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
   public class CreateTypeBindingModel
    {
        [Required]
        public int TypeId { get; set; }

        [Required]
        public int GatewayId { get; set; }

        [Required]
        public int ObjectId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        public string TypeCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string TypeName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

    }
}
