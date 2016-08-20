using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMM.APEC.WebAPI.Logging;
using System.Web.Http.ExceptionHandling;


namespace JMM.APEC.WebAPI.ExceptionHandling
{
    public class SlabLogExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            LogWriter.Log.UnHandledException(
                context.Request.Method.ToString(),
                context.Request.RequestUri.ToString(),
                context.Exception.Message);
        }

     
     }
}
