using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.Core.Interfaces;
using System.Data.SqlClient;
using JMM.APEC.WebAPI.Logging;
using System.Reflection;
using System.Data.Common;
using Microsoft.SqlServer.Server;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.DAL;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class SystemStatusDao : ISystemStatusDao
    {
        private string _databaseName;
        private Db db;

        public SystemStatusDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a System_Status object based on DataReader
        static Func<IDataReader, System_StatusGateway> Make = reader =>
           new System_StatusGateway
           {
               GatewayId = reader["gatewayID"].AsInt(),
               Status = new System_Status
               {
                   Id = reader["statusID"].AsInt(),
                   Code = reader["Code"].AsString(),
                   Value = reader["value"].AsString(),
                   Description = reader["description"].AsString(),
                   Active = reader["statusIsActive"].AsBool(),
                   StatusType = new System_StatusType
                   {
                       Id = reader["statusTypeID"].AsInt(),
                       Code = reader["statusTypeCode"].AsString(),
                       Name = reader["statusTypeName"].AsString()
                   }                   

               }
              
           };

        DbParameter[] Take(System_Status status)
        {
            var parameters = new[]{
                new SqlParameter(){ ParameterName="@inId", DbType = DbType.Int32, Value = status.Id },
                new SqlParameter(){ ParameterName="@inCode", DbType = DbType.String, Value = status.Code },
                new SqlParameter(){ ParameterName="@inValue", DbType = DbType.String, Value = status.Value },
                new SqlParameter(){ ParameterName="@inDescription", DbType = DbType.String, Value = status.Description },
                new SqlParameter(){ ParameterName="@inActive", DbType = DbType.Boolean, Value = status.Active },
                new SqlParameter(){ ParameterName="@inSortOrder", DbType = DbType.Int32, Value = status.SortOrder },
                new SqlParameter(){ ParameterName="@inStatusTypeId", DbType = DbType.Int32, Value = status.StatusTypeId }
                //new SqlParameter(){ ParameterName="@inGatewayId", DbType = DbType.Int32, Value = status.GatewayId }
            };

            return parameters;
        }

   
       public void InsertStatus(System_Status status)
       {
           string storedProcName = "System_insertStatus";

           status.Id = db.Insert(storedProcName, Take(status));

           return;
       }

       public void UpdateStatus(System_Status status)
       {
           string storedProcName = "System_updateStatus";

           db.Update(storedProcName, Take(status));

           return;
       }

       public void DeleteStatus(System_Status status)
       {
           string storedProcName = "System_deleteStatus";

           db.Delete(storedProcName, Take(status));

           return;
       }

        public List<System_Status> GetAlarmActionStatus()
        {
            List<System_Status> ActionStatusList = new List<System_Status>();

            System_Status actionstatus1 = new System_Status();

            actionstatus1.Id = 1;
            actionstatus1.Active = true;
            actionstatus1.Value = "In progress";

            System_Status actionstatus2 = new System_Status();

            actionstatus2.Id = 2;
            actionstatus2.Active = true;
            actionstatus2.Value = "Inactive";

            System_Status actionstatus3 = new System_Status();

            actionstatus3.Id = 3;
            actionstatus3.Active = true;
            actionstatus3.Value = "Dispatched";

            System_Status actionstatus4 = new System_Status();

            actionstatus4.Id = 4;
            actionstatus4.Active = true;
            actionstatus4.Value = "Pending";

            System_Status actionstatus5 = new System_Status();

            actionstatus5.Id = 1;
            actionstatus5.Active = true;
            actionstatus5.Value = "Resolved";

            ActionStatusList.Add(actionstatus1);
            ActionStatusList.Add(actionstatus2);
            ActionStatusList.Add(actionstatus3);
            ActionStatusList.Add(actionstatus4);
            ActionStatusList.Add(actionstatus5);


            return ActionStatusList;

        }


        public List<System_Status> GetAlarmResolutionStatus()
        {
            List<System_Status> ResolutionStatusList = new List<System_Status>();

            System_Status resolutionstatus1 = new System_Status();

            resolutionstatus1.Id = 1;
            resolutionstatus1.Active = true;
            resolutionstatus1.Value = "JMM Resolved Remotely";

            System_Status resolutionstatus2 = new System_Status();

            resolutionstatus2.Id = 2;
            resolutionstatus2.Active = true;
            resolutionstatus2.Value = "Self Cleared";

            System_Status resolutionstatus3 = new System_Status();

            resolutionstatus3.Id = 3;
            resolutionstatus3.Active = true;
            resolutionstatus3.Value = "Site Manager Resolved";

            System_Status resolutionstatus4 = new System_Status();

            resolutionstatus4.Id = 4;
            resolutionstatus4.Active = true;
            resolutionstatus4.Value = "Dispatched Vendor";
           
            ResolutionStatusList.Add(resolutionstatus1);
            ResolutionStatusList.Add(resolutionstatus2);
            ResolutionStatusList.Add(resolutionstatus3);
            ResolutionStatusList.Add(resolutionstatus4);        


            return ResolutionStatusList;

        }

        public List<System_StatusGateway> GetStatus(ApplicationSystemUser user, string Gateways, string StatusTypeCode, string StatusCode )
        {
            string storedProcName = "System_getStatus";

            if (StatusTypeCode == "null" || StatusTypeCode == "")
            {
                StatusTypeCode = null;
            }

            if (StatusCode == "null" || StatusCode == "")
            {
                StatusCode = null;
            }

            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intUserGatewayList = (from g in user.Gateways select g.PortalGatewayId).ToList();

            List<int> intFinalGatewayList = null;

            if (intGatewayList == null)
            {
                intFinalGatewayList = intUserGatewayList;
            }
            else
            {
                intFinalGatewayList = (from a in intUserGatewayList join b in intGatewayList on a equals b select a).ToList();
            }

            List<SqlDataRecord> GatewayList = DbHelpers.MakeParamRecordList(intFinalGatewayList, "Id");          

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@gatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList },
                new SqlParameter(){ ParameterName="@inStatusTypeCode",DbType = DbType.String, Value = StatusTypeCode },
                new SqlParameter(){ ParameterName="@inStatusCode", DbType = DbType.String, Value = StatusCode }               
                
            };

            IEnumerable<System_StatusGateway> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + " for Gateways '{0}', StatusCode '{1}', StatusTypeCode '{2}'.", Gateways, StatusCode, StatusTypeCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);

                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();


        }

    }
}
