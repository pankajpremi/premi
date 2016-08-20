using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class TankEventDto
    {
        public string TypeName { get; set; }
        public DateTime? EventDate { get; set; }
        public decimal VolumeAmnt { get; set; }
        public decimal HeightAmnt { get; set; }
    }
}