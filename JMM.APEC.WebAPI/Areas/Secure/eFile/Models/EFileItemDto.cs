using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class EFileItemDto
    {
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public string NodeType { get; set; }
        public List<EFileItemDto> SubNodes { get; set; }
    }
}