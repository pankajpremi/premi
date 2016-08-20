using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class CreateGatewayBindingModel
    {
        public int? StatusId { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Short Name")]
        public string GatewayShortName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string GatewayName { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Effective End Date")]
        public DateTime? EffectiveEndDate { get; set; }
        [Required]
        public int AppChangeUserId { get; set; }
    }
}