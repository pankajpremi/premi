using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserPortalDao
    {
        List<UserPortal> FindByUserName(string username);
        List<UserPortal> FindByUserId(int userId);
        List<UserPortal> FindByUserIdAndPortal(int userId, int portalId);
        int Insert(int userId, int portalId);
    }
}
