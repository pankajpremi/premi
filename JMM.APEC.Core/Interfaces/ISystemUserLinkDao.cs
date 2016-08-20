using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface ISystemUserLinkDao
    {

        List<System_UserLink> GetFavLink(ApplicationSystemUser user);

    }
}
