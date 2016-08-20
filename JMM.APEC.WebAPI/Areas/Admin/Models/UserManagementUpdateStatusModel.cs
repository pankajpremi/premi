using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class UserManagementUpdateStatusModel
    {
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
    }
}