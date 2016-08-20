//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Tracing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JMM.APEC.WebAPI.Logging
//{
//   public class DALLogWriter : EventSource
//    {

//        private static readonly Lazy<DALLogWriter> Instance = new Lazy<DALLogWriter>(() => new DALLogWriter());

//        public static DALLogWriter dalLog
//        {
//            get { return Instance.Value; }
//        }

//        public class Keywords
//        {
//            public const EventKeywords Database = (EventKeywords)16;
           
//         }

//        public class Tasks
//        {
//            public const EventTask DatabaseRead = (EventTask)1;
//            public const EventTask DatabaseWrite = (EventTask)2;
//            public const EventTask DatabaseRetry = (EventTask)3;
                        
//        }


//        private const int LogDBInfo = 310; 
//        private const int LogDBErrors = 311;
//        private const int LogDBRetryErrors = 312;
//        private const int LogDBNoData = 313;

//        [Event(314, Message = "Method {0}, output params = {1}", Level = EventLevel.Informational,Task = Tasks.DatabaseRead, Keywords= Keywords.Database)]
//        public void  DatabaseOutputParams(string methodName, string paramString)
//        {
//            if (this.IsEnabled()) this.WriteEvent(310, methodName, paramString);
//        }

       
//        [Event(315, Message = "DB Retry in method {0}, MiscInfo = {1}, TryCount = {2}, " + "RetrySleepTime = {3}, ex = {4}", Level = EventLevel.Error, Keywords = Keywords.Database, Task = Tasks.DatabaseRead)]
//        public void DatabaseRetry(string methodName, string miscInfo, int tryCount, int sleepTime,
//          string exceptionInfo)
//        {
//            if (this.IsEnabled(EventLevel.Error, Keywords.Database)) this.WriteEvent(311, methodName, miscInfo, tryCount, sleepTime,
//              exceptionInfo);
//        }


//        [Event(316, Message = "Error converting the database fields to output parameters in method {0}. " + "ExtraInfo = {1}, ex = {2}",
//          Level = EventLevel.Error, Keywords = Keywords.Database, Task = Tasks.DatabaseRead)]
//        internal void DatabaseFieldConversionError(string methodName, string extraInfo, string errorMessage)
//        {
//            if (this.IsEnabled(EventLevel.Error, Keywords.Database)) this.WriteEvent(312, methodName, extraInfo, errorMessage);
//        }

      
//       [Event(LogDBInfo, Message = "Message from method {0} = {1}", Level = EventLevel.Informational, Task = Tasks.DatabaseRead)]
//        public void WriteDBInfoMsg(string methodName, string Message)
//        {
//            if (this.IsEnabled()) this.WriteEvent(LogDBInfo, methodName, Message);
//        }

//      [Event(LogDBNoData, Message = "Method {0}, no data found. Input Info = {1}", Level = EventLevel.Warning, Keywords = Keywords.Database, Task = Tasks.DatabaseRead)]
//      public void DatabaseRecordsNotFound(string Methodname,string errorMessage)
//      {
//          if (this.IsEnabled(EventLevel.Warning, Keywords.Database))
//          {
//              this.WriteEvent(LogDBNoData, Methodname, errorMessage);

//          }
//      }
       
//   }
//}
