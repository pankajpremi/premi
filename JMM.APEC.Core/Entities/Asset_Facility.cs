using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Facility
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string AKAName { get; set; }
        public bool? IsDeleted { get; set; }

        public int GatewayId { get; set; }
        public Asset_Gateway Gateway { get; set; }

        public int? AddressId { get; set; }
        public Asset_Address Address { get; set; }

        public int? StatusId { get; set; }
        public System_Status Status { get; set; }

        public int? TypeId { get; set; }
        public System_Type Type { get; set; }

        public int AppChangeUserId { get; set; }
        public string PrimaryPhoneNumber { get; set; }
       
    }
}
