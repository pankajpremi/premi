using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class System_Category
    {
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public bool Active { get; set; }
        public int SortOrder { get; set; }
        // Foreign Key
        public int ObjectId { get; set; }
        // Navigation property
        public System_Object Object { get; set; }
        // Foreign Key
        public int GatewayId { get; set; }
        // Navigation property
        public Asset_Gateway Gateway { get; set; }
    }
}
