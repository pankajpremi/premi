using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.WebAPI.Logging;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class SystemUserLinkDao : ISystemUserLinkDao
    {

        private string _databaseName;
        private Db db;

        public SystemUserLinkDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        // creates a Asset_Facility object based on DataReader
        static Func<IDataReader, System_UserLink> Make = reader =>
           new System_UserLink
           {
               UserId = reader["UserId"].AsId(),
               LinkName = reader["linkname"].AsString(),
               LinkUrl = reader["linkurl"].AsString()                          
           };


        public List<System_UserLink> GetFavLink(ApplicationSystemUser user)
        {
            string storedProcName = "UserFavLinks";

            var intUserGatewayList = (from g in user.Gateways select g.PortalGatewayId).ToList();

            List<int> intFinalGatewayList = null;

           intFinalGatewayList = intUserGatewayList;
           

            List<SqlDataRecord> GatewayList = DbHelpers.MakeParamRecordList(intFinalGatewayList, "Id");   

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@gatewayIDs", SqlDbType = SqlDbType.Structured, Value = GatewayList }
            };

            IEnumerable<System_UserLink> result = db.Read(storedProcName, Make, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for User '{0}'.", user.UserName);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            LogWriter.Log.WriteDBInfoMsg(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), result.Count().ToString());

            return result.ToList();
        }

   
    }
}
