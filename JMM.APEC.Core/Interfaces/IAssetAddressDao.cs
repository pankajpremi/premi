
using System.Collections.Generic;

namespace JMM.APEC.Core.Interfaces
{
     public interface  IAssetAddressDao
    {
         List<Asset_Address> GetAddress(int AddressId);
        int InsertAddress(Asset_Address Address);
        int UpdateAddress(Asset_Address Address);
    }
}
