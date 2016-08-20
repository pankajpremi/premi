using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JMM.APEC.Efile
{
    public class Service_EfileNode
    {
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<Service_EfileNode> SubNodes { get; set; }

        public static Service_EfileNode Parse(XElement value)
        {
            return new Service_EfileNode
            {
                EntityId = int.Parse(value.Attribute("id").Value),
                Name = value.Attribute("name").Value,
                Description = value.Attribute("description").Value,
                Type = value.Attribute("type").Value,
                SubNodes = value.Elements().Select(newvalue => Parse(newvalue)).ToList()
            };
        }

    }
}
