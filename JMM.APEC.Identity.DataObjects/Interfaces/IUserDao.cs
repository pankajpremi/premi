using JMM.APEC.Core;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IUserDao
    {
        string GetUserName(int userId);
        int GetUserId(string userName);
        User GetUserById(int userId, int portalId);
        List<User> GetUserByName(string userName);
        List<User> GetUserByEmail(string email);
        List<User> GetUsers(ApplicationSystemUser User, string Gateways, string Statuses, string SearchText, int? PageNum, int? PageSize, string SortField, string SortDirection);
        string GetPasswordHash(int userId);
        int SetPasswordHash(int userId, string passwordHash);
        string GetSecurityStamp(int userId);
        int Insert(User user);
        int Delete(int userId);
        int Update(User user);
        int UpdateWithProfile(User user);

    }
}
