using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface ISystemMessageDao
    {

        List<System_MessageList> GetMessage(int? MessageId, string ObjectCode, string TypeCode, string UserToId, string UserFromId, string Search, int? Seed, int? Limit, string Sortcolumn, string Sortorder);
        System_MessageAssignmentList InsertMessageAssignment(ApplicationSystemUser user,System_MessageAssignmentList Message);
        List<System_MessageAssignment> GetMessageAssignment(string ObjectCode, string EntityId, string MessageId, string TypeId);

        System_MessageAssignmentList UpdateMessageAssignment(ApplicationSystemUser user,System_MessageAssignmentList Message);
        int DeleteMessage(int MessageId);
        System_Message InsertUserMessage(ApplicationSystemUser user, int ToUserId, string Subject, string Message);

    }
}

