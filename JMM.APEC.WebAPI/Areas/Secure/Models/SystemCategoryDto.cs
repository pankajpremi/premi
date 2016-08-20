using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Models
{
    public class SystemCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public bool Active { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectName { get; set; }
    }
}
