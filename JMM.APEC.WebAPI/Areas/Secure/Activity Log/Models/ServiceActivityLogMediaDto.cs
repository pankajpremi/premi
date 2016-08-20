using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class ServiceActivityLogMediaDto
    {

        public int ActivityLogId { get; set; }
        public int MediaId { get; set; }              
        public string FileName { get; set; }
    
    }
}