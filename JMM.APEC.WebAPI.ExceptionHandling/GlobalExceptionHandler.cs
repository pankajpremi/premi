using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Net.Http;
using System.Threading;

namespace JMM.APEC.WebAPI.ExceptionHandling
{
    public class MyGlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {

            if (context.Exception is UnauthorizedAccessException)
            {
                
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "UnauthorizedAccessException"
                };

                context.Result = new ErrorMessageResult(context.Request, resp);

            }

            if (context.Exception is ArgumentNullException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "ArgumentNullException"
                };

                context.Result = new ErrorMessageResult(context.Request, resp);
            }

            if (context.Exception is NullReferenceException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "NullReferenceException"
                };

                context.Result = new ErrorMessageResult(context.Request, resp);
            }

            if (context.Exception is NotImplementedException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "NotImplementedException"
                };

                context.Result = new ErrorMessageResult(context.Request, resp);
            }

            
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Server Error"),
                    ReasonPhrase = "Internal exception has occured"
                };

                context.Result = new ErrorMessageResult(context.Request, resp);
            }

        }

        //public override bool ShouldHandle(ExceptionHandlerContext context)
        //{
        //    return true;
        //}


        public class ErrorMessageResult : IHttpActionResult
        {
            private HttpRequestMessage _request;
            private HttpResponseMessage _httpResponseMessage;


            public ErrorMessageResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
            {
                _request = request;
                _httpResponseMessage = httpResponseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_httpResponseMessage);
            }
        }
    }
}
