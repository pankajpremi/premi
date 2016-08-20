using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface ISystemObjectDao
    {
        //List<Asset_PhoneAssignment> GetPhoneList(string ObjectCode, string objectId, string EntityId, string TypeId);
        //List<Asset_ObjectContact> GetContactList(string ObjectCode, string objectId, string EntityId, string TypeId);
        //List<System_ObjectMessage> GetMessageList(string ObjectCode, string objectId, string EntityId, string TypeId);
        void InsertObject(System_Object obj);
        void UpdateObject(System_Object obj);
        void DeleteObject(System_Object obj);
    }
}
