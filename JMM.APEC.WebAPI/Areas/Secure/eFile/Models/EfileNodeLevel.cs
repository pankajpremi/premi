using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class EfileNodeLevel
    {
        public string NodeType { get; set; }
        public string NodeText { get; set; }
        public int EntityId { get; set; }
        public int LevelDepth { get; set; }
    }
}