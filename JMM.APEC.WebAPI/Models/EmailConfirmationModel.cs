using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class EmailConfirmationModel
    {
        public string PortalName { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
    }
}