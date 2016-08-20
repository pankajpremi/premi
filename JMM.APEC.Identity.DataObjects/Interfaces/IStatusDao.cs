using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public interface IStatusDao
    {
        List<Status> FindStatuses();
        Status FindStatusByCode(string Code);
        Status FindStatusById(int StatusId);
        Status GetStatusByUserId(int UserId);
    }
}
