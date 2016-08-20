using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class EventTrackerBindingModel
    {
       
            public List<string> SortFields { get; set; }
            public List<string> SortDirections { get; set; }
            public string Gateways { get; set; }
            public string Facilities { get; set; }
            public string Statuses { get; set; }
            public string Categories { get; set; }
            public string Types { get; set; }
            public string Subtypes { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }


    }
}