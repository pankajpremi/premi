using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Admin.Models
{
    public class UserFacilitiesUpdateModel
    {
        public List<UserFacilityModelDto> Facilities { get; set; }
    }
}