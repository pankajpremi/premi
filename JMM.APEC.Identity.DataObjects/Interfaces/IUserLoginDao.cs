using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserLoginDao
    {
        int Delete(User user, UserLogin login);
        int Delete(int userId);
        int Insert(User user, UserLogin login);
        int FindUserIdByLogin(UserLogin login);
        List<UserLogin> FindByUserId(int userId);

    }
}
