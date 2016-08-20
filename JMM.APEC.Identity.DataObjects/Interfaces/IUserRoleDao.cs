using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserRoleDao
    {
        List<string> FindByUserId(int userId);
        int Delete(int userId);
        int Insert(User user, int roleId);

    }
}
