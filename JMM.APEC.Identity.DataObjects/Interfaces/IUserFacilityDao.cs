using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserFacilityDao
    {
        List<UserFacilityModel> FindByUserIdAndGateway(int portalId, int portalGatewayId, int userId);
    }
}
