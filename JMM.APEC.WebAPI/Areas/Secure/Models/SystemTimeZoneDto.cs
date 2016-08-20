using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemTimeZoneDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Offset { get; set; }
        public string GMT { get; set; }
    }
}