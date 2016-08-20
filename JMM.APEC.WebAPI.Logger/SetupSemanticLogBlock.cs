using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;



namespace JMM.APEC.WebAPI.Logging
{
    public static class SetupSemanticLogBlock
    {

        public static void SetupSemanticLoggingApplicationBlock()
        {
            //EventTracing setup
            string logConnString = System.Configuration.ConfigurationManager.ConnectionStrings["Apec2Identity"].ToString();

            var sqlListener1 = SqlDatabaseLog.CreateListener("WebAPI", logConnString);

           // var sqlListener3 = SqlDatabaseLog.CreateListener("DAL", logConnString);
                
                //new ObservableEventListener();
            //SinkSubscription<SqlDatabaseSink> subscription = sqlListener3.LogToSqlDatabase("DAL", logConnString);
            
            //get web.config value for logging level
            bool DBOutParams = System.Configuration.ConfigurationManager.AppSettings["LogDatabaseOutParams"].ToLower() ==
                         "true";
            bool DBErrors = System.Configuration.ConfigurationManager.AppSettings["LogDatabaseError"].ToLower() ==
                         "true";
            bool DBInfo = System.Configuration.ConfigurationManager.AppSettings["LogDatabaseInfo"].ToLower() ==
                         "true";
            bool DBNodata = System.Configuration.ConfigurationManager.AppSettings["LogDatabaseNodata"].ToLower() ==
                           "true";
            bool DBRetry = System.Configuration.ConfigurationManager.AppSettings["LogDatabaseRetry"].ToLower() ==
                           "true";
            bool InParams = System.Configuration.ConfigurationManager.AppSettings["LogInputParams"].ToLower() ==
                          "true";
            bool EntryPoint = System.Configuration.ConfigurationManager.AppSettings["LogEntryPoint"].ToLower() ==
                          "true";
           
            bool LogExcep = System.Configuration.ConfigurationManager.AppSettings["LogExceptions"].ToLower() ==
                          "true";
            bool LogReqs = System.Configuration.ConfigurationManager.AppSettings["LogRequests"].ToLower() ==
                         "true";
            bool LogExecu = System.Configuration.ConfigurationManager.AppSettings["LogPerformance"].ToLower() ==
                         "true";

            bool LogUserLogins = true;

            //Enable the level of logging based on settings in web.config
            if (DBNodata)
           {
               sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Warning, Keywords.All);
           }


            if (DBRetry)
           {
               sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Error, Keywords.All);
           }
            if (DBErrors)
            {
                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Error, Keywords.All);
            }

            if (DBOutParams)
            {

                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Informational, Keywords.All);
            }

            if (DBInfo)
            {

                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Informational);
            }

            if (InParams)
            {
                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Error, Keywords.All);

            }

            if (EntryPoint)
            {
                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Informational, Keywords.All);
            }

           
            if (LogExcep)
            {
                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Error, Keywords.All);                

            }
            if (LogReqs)
            {
                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Informational);

            }
            if (LogExecu)
            {
                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Informational, Keywords.All);

            }

            if (LogUserLogins)
            {
                sqlListener1.EnableEvents(LogWriter.Log, EventLevel.Informational, Keywords.All);

            }
        }
    }
}
