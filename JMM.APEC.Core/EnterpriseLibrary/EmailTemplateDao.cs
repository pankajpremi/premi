using JMM.APEC.Core.Interfaces;
using JMM.APEC.DAL.EnterpriseLibrary;
using JMM.APEC.WebAPI.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.EnterpriseLibrary
{
    public class EmailTemplateDao : IEmailTemplateDao
    {
        private string _databaseName;
        private Db db;

        public EmailTemplateDao(string DatabaseName)
        {
            _databaseName = DatabaseName;
            db = new Db(_databaseName);
        }

        static Func<IDataReader, System_EmailTemplate> MakeTemplate = reader =>
           new System_EmailTemplate
           {
               Code = reader["templateCode"].AsString(),
               ContainerUrl = reader["templateContainerURL"].AsString(),
               Description = reader["templateDesc"].AsString(),
               EnableSsl = reader["enableSSL"].AsBool(),
               FromAddress = reader["fromAddress"].AsString(),
               FromName = reader["fromName"].AsString(),
               Password = reader["password"].AsString(),
               Port = reader["port"].AsInt(),
               SmtpServer = reader["smtpServer"].AsString(),
               Subject = reader["subject"].AsString(),
               TemplateUrl = reader["templateURL"].AsString(),
               Username = reader["username"].AsString()
           };

        static Func<IDataReader, System_EmailTransport> MakeTransport = reader =>
           new System_EmailTransport
           {
               Code = reader["Code"].AsString(),
               Description = reader["Description"].AsString(),
               EnableSsl = reader["EnableSSL"].AsBool(),
               Password = reader["password"].AsString(),
               Port = reader["port"].AsInt(),
               SmtpServer = reader["smtpServer"].AsString(),
               Username = reader["username"].AsString()
           };

        public List<System_EmailTransport> GetTransport(string TransportCode)
        {
            string storedProcName = "System_getEmailTemplate";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@emailCode", DbType = DbType.String, Value = TransportCode },
            };

            IEnumerable<System_EmailTransport> result = db.Read(storedProcName, MakeTransport, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for Message Transport '{0}'.", TransportCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            return result.ToList();
        }

        public List<System_EmailTemplate> GetTemplate(string TemplateCode, int? PortalId, int? GatewayId)
        {
            string storedProcName = "System_getEmailTemplate";

            var parameters = new[]{
                new SqlParameter(){ ParameterName="@portalID", DbType = DbType.Int32, Value = PortalId },
                new SqlParameter(){ ParameterName="@gatewayID", DbType = DbType.Int32, Value = GatewayId },
                new SqlParameter(){ ParameterName="@emailCode", DbType = DbType.String, Value = TemplateCode },
            };

            IEnumerable<System_EmailTemplate> result = db.Read(storedProcName, MakeTemplate, parameters);

            if (result == null || result.Count() == 0)
            {
                string errorMessage = string.Format("Error - record not found " + "for Email Template '{0}'.", TemplateCode);
                LogWriter.Log.DatabaseRecordsNotFound(LogUtility.GetMethodDetails(MethodBase.GetCurrentMethod()), errorMessage);
                return null;
            }

            return result.ToList();
        }
    }
}
