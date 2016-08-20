using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.WebAPI.Logging
{

    public class LogWriter : EventSource
    {

        private static readonly Lazy<LogWriter> Instance = new Lazy<LogWriter>(() => new LogWriter());

        public static LogWriter Log
        {
            get { return Instance.Value; }
        }

        public class Keywords
        {
            public const EventKeywords ParameterError = (EventKeywords)1;
            public const EventKeywords EntryPoint = (EventKeywords)2;
            public const EventKeywords Performance = (EventKeywords)4;
            public const EventKeywords Exceptions = (EventKeywords)8;
            public const EventKeywords LoginAttempts = (EventKeywords)32;
            public const EventKeywords Database = (EventKeywords)16;

        }

        public class Tasks
        {
            public const EventTask DatabaseRead = (EventTask)1;
            public const EventTask DatabaseWrite = (EventTask)2;
            public const EventTask DatabaseRetry = (EventTask)3;

        }

              
        private const int LogInputParams = 301; //level=Warning,Keyword=ParameterError
        private const int LogEntryPoint = 302; //level=informational,Keyword=EntryPoint
        private const int LogExceptions = 303; //level=error
        private const int LogRequest = 304; //level=Informational
        private const int LogResponse = 305; //level=Informational
        private const int LogExectionTime = 306;
        private const int LogInfo = 307;
        private const int LogLoginAttempts = 308;

        private const int LogDBInfo = 310;
        private const int LogDBErrors = 311;
        private const int LogDBRetryErrors = 312;
        private const int LogDBNoData = 313;


       [Event(LogInputParams, Message = "Parameter error in method {0}, msg = {1}", Level = EventLevel.Error,Keywords = Keywords.ParameterError)]
        public void ParameterError(string methodName, string errorMessage)
        {
            if (this.IsEnabled(EventLevel.Error,Keywords.ParameterError)) this.WriteEvent(LogInputParams, methodName, errorMessage);
        }

        [Event(LogEntryPoint, Message = " User: {0} Method: {1} Called/entry point.", Level = EventLevel.Informational, Keywords = Keywords.EntryPoint)]
        public void MethodEntryPoint(string Username,string Input)
        {
            if (this.IsEnabled(EventLevel.Informational, Keywords.EntryPoint))
            {
                this.WriteEvent(LogEntryPoint, Username,Input);

            }
        }

       [Event(LogExceptions, Message = "Unexpected exception in Method {0}, URI = {1}, Exception = {2}",Level = EventLevel.Error)]
        public void UnHandledException(string methodName, string miscInfo, string exceptionInfo)
        {
            if (this.IsEnabled()) this.WriteEvent(LogExceptions, methodName, miscInfo, exceptionInfo);
        }

        [Event(LogRequest, Message = "{0}",Level = EventLevel.Informational)]
        public void RequestLog(string MethodName, string Details)
        {
            if (this.IsEnabled()) this.WriteEvent(LogRequest, MethodName, Details);
        }

        [Event(LogResponse, Message = "{0}", Level = EventLevel.Informational)]
        public void ResponseLog(string MethodName, string Details)
        {
            if (this.IsEnabled()) this.WriteEvent(LogResponse, MethodName, Details);
        }

        [Event(LogExectionTime, Message = "{0}", Level = EventLevel.Informational, Keywords= Keywords.Performance)]
        public void ExecutionTimeLog(string data)
        {
            if (this.IsEnabled()) this.WriteEvent(LogExectionTime, data);
        }

        [Event(LogInfo, Message = "User: {0} Info: {1}", Level = EventLevel.Informational)]
        public void LogMessage(string Username,string Info)
        {
            if (this.IsEnabled()) this.WriteEvent(LogInfo, Username,Info);
        }

        [Event(LogLoginAttempts, Message = "User Login Attempts - User: {0}, Success: {1}, IP Address: {2}, TimeStamp: {3}", Level = EventLevel.Informational, Keywords = Keywords.LoginAttempts)]
        public void LoginAttemptsLog(string UserName, Boolean Success, string IP, DateTime TimeStamp)
        {
            if (this.IsEnabled()) this.WriteEvent(LogLoginAttempts, UserName, Success, IP, TimeStamp);
        }

        public static string FormatException(Exception exception)
        {
            return exception.GetType().ToString() + Environment.NewLine + exception.Message;
        }

        [Event(314, Message = "Method {0}, output params = {1}", Level = EventLevel.Informational, Task = Tasks.DatabaseRead, Keywords = Keywords.Database)]
        public void DatabaseOutputParams(string methodName, string paramString)
        {
            if (this.IsEnabled()) this.WriteEvent(310, methodName, paramString);
        }


        [Event(315, Message = "DB Retry in method {0}, MiscInfo = {1}, TryCount = {2}, " + "RetrySleepTime = {3}, ex = {4}", Level = EventLevel.Error, Keywords = Keywords.Database, Task = Tasks.DatabaseRead)]
        public void DatabaseRetry(string methodName, string miscInfo, int tryCount, int sleepTime,
          string exceptionInfo)
        {
            if (this.IsEnabled(EventLevel.Error, Keywords.Database)) this.WriteEvent(311, methodName, miscInfo, tryCount, sleepTime,
              exceptionInfo);
        }


        [Event(316, Message = "Error converting the database fields to output parameters in method {0}. " + "ExtraInfo = {1}, ex = {2}",
          Level = EventLevel.Error, Keywords = Keywords.Database, Task = Tasks.DatabaseRead)]
        internal void DatabaseFieldConversionError(string methodName, string extraInfo, string errorMessage)
        {
            if (this.IsEnabled(EventLevel.Error, Keywords.Database)) this.WriteEvent(312, methodName, extraInfo, errorMessage);
        }


        [Event(LogDBInfo, Message = "Message from method {0} = {1}", Level = EventLevel.Informational, Task = Tasks.DatabaseRead)]
        public void WriteDBInfoMsg(string methodName, string Message)
        {
            if (this.IsEnabled()) this.WriteEvent(LogDBInfo, methodName, Message);
        }

        [Event(LogDBNoData, Message = "Method {0}, no data found. Input Info = {1}", Level = EventLevel.Warning, Keywords = Keywords.Database, Task = Tasks.DatabaseRead)]
        public void DatabaseRecordsNotFound(string Methodname, string errorMessage)
        {
            if (this.IsEnabled(EventLevel.Warning, Keywords.Database))
            {
                this.WriteEvent(LogDBNoData, Methodname, errorMessage);

            }
        }


        }


       
    }
