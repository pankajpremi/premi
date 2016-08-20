using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Models
{
    public class AssetGatewayDto
    {
        public int IdentityId { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int AddressId { get; set; }
        public DateTime ActiveEndDate { get; set; }
        public int StatusId { get; set; }

    }
}