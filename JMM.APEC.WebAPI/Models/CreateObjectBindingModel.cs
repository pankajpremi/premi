using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
    public class CreateObjectBindingModel
    {
        [Required]
        public int ObjectId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        public string ObjectCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string ObjectName { get; set; }

    }
}
