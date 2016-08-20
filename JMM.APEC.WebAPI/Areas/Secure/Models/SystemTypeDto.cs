using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
   public class SystemTypeDto
    {

        public int ObjectId { get; set; }
        public int GatewayId { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }

    }
}
