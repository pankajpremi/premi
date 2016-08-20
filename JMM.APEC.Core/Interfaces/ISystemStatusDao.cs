using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
    public interface ISystemStatusDao
    {
        //List<System_StatusGateway> GetStatus(string GatewayId, string StatusTypeCode, string StatusCode);
        void InsertStatus(System_Status status);
        void UpdateStatus(System_Status status);
        void DeleteStatus(System_Status status);

        List<System_Status> GetAlarmActionStatus();
        List<System_Status> GetAlarmResolutionStatus();

        List<System_StatusGateway> GetStatus(ApplicationSystemUser user,string GatewayIds, string StatusTypeCode, string StatusCode);
    }
}
