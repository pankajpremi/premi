using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_Media
    {

        public int MediaId { get; set; }

        public int EntityId { get; set; }

        public string MediaName { get; set; }

        public string MediaDescription { get; set; }

        public string MediaFilePath { get; set; }

        public string MediaFileName { get; set; }

        public double? MediaFileSize { get; set; }

        public bool MediaDeleted { get; set; }

        public DateTime? LogDate { get; set; }

        public int AppChangeUserId { get; set; }


        public int ObjectId{ get; set; }
        public System_Object obj { get; set; }

        public int TypeId { get; set; }
        public System_Type Type { get; set; }

        public int CategoryId { get; set; }
        public System_Category Category { get; set; }

}
}
