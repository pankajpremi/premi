using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class CreateStatusBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int GatewayId { get; set; }

        [Required]
        public int StatusTypeId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Value")]
        public string Value { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}