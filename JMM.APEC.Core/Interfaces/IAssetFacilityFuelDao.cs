using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface IAssetFacilityFuelDao
    {
       
        List<Asset_FacilityFuel> GetFacilityFuel(int FacilityId);
        int InsertFacilityFuel(Asset_FacilityFuel FacFuel);
        int UpdateFacilityFuel(Asset_FacilityFuel FacFuel);
    }
}
