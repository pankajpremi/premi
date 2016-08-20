using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.IMS
{
    public class Service_TankCompartmentEvent
    {
        public int EventId { get; set; }
        public int TankCompartmentId { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public DateTime? EventDateTime { get; set; }
        public decimal VolumeAmt { get; set; }
        public decimal HeightAmt { get; set; }

    }
}
