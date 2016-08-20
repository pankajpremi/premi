using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using JMM.APEC.BusinessObjects.Entities;

namespace JMM.APEC.Core.Interfaces
{
    public interface IAssetPortalDao
    {

        List<Asset_Portal> GetPortal(int? PortalId, bool? IsActive);
        void InsertPortal(Asset_Portal portal);
        void UpdatePortal(Asset_Portal portal);
        void DeletePortal(Asset_Portal portal);
       
    }
}
