using JMM.APEC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Efile.Interfaces
{
    public interface IServiceEfilesDao
    {
        List<Service_EfileNode> GetEFileItems(ApplicationSystemUser user, string Gateways, string SortOption, string Keyword);
    }
}
