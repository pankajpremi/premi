using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
   public class Asset_Sensor
    {
        public int SensorId { get; set; }
        public int? ObjectId { get; set; }
        public int EntityId { get; set; }
        public int? Location { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int AppChangeUserId { get; set; }

        public int CategoryId { get; set; }
        public System_Category Category { get; set; }

        public int TypeId { get; set; }
        public System_Type Type { get; set; }

        public int DeviceTypeId { get; set; }
        public System_Type DeviceType { get; set; }

        public int SubTypeId { get; set; }
        public System_SubType SubType { get; set; }
    }
}
