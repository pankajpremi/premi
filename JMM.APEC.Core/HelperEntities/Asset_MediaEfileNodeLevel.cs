using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core
{
    public class Asset_MediaEfileNodeLevel
    {
        public string NodeType { get; set; }
        public string NodeText { get; set; }
        public int EntityId { get; set; }
        public bool SubLevels { get; set; }
        public int NodeDepth { get; set; }
    }
}
