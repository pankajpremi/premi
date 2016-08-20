using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserClaimDao
    {
        List<UserClaim> FindByUserId(int userId);
        int Delete(int userId);
        int Delete(User user, UserClaim claim);
        int Insert(UserClaim userClaim, int userId);

    }
}
