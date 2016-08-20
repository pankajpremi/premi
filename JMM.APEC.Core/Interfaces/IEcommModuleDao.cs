//using JMM.APEC.BusinessObjects;
//using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface IEcommModuleDao
    {
        List<Ecomm_Module> GetModulesForUser(ApplicationSystemUser user, int? gatewayId);
    }
}
