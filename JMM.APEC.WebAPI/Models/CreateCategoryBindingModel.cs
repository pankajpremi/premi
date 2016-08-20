using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
   public class CreateCategoryBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int GatewayId { get; set; }

        [Required]
        public int ObjectId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        public string CategoryCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Active")]
        public string IsActive { get; set; }

    }
}
