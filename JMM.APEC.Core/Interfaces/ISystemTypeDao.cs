using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JMM.APEC.Core.Interfaces
{
    public interface ISystemTypeDao
    {
        List<System_Type> GetType(ApplicationSystemUser user, int? ObjectId, string ObjectCode, string GatewayIDs, string TypeCode);
        //void InsertType(System_Type type);
        //void UpdateType(System_Type type);
        //void DeleteType(System_Type type);
    }
}
