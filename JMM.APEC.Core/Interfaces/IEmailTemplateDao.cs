using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface IEmailTemplateDao
    {
        List<System_EmailTemplate> GetTemplate(string TemplateCode, int? PortalId, int? GatewayId);
        List<System_EmailTransport> GetTransport(string TransportCode);


    }
}
