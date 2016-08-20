using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_SensorAssignment
    {
        public int SensorAssignmentId { get; set; }
        public int EntityId { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public int SensorId { get; set; }
        public Asset_Sensor Sensor { get; set; }
        public int ObjectId { get; set; }
        public System_Object obj { get; set; }
    }
}
