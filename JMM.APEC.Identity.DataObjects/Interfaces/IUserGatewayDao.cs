using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserGatewayDao
    {
        List<UserGateway> FindByUserId(int userId, int portalId);
        int Insert(int userId, int gatewayId, int portalId);
    }
}
