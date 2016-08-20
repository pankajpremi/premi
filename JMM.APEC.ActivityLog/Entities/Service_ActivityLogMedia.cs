using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.ActivityLog
{
    public class Service_ActivityLogMedia
    {
        public int ActivityLogId { get; set; }
        public int MediaId { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public double FileSize { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LogDateTime { get; set; }
        public int AppChangeUserId { get; set; }
    }
}
