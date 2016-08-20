using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IRoleDao
    {
        int Delete(int roleId);
        int Insert(Role role);
        List<Role> GetRoles();
        string GetRoleName(int roleId);
        int GetRoleId(string roleName);
        Role GetRoleById(int roleId);
        Role GetRoleByName(string roleName);
        int Update(Role role);
        Role GetRoleByUserId(int userId);

    }
}
