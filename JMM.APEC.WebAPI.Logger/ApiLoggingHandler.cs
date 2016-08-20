using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using System.Configuration;

namespace JMM.APEC.WebAPI.Logging
{

    //public class ApiLogHandler : DelegatingHandler
    //{
    //    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        var apiLogEntry = CreateApiLogEntryWithRequestData(request);
    //        if (request.Content != null)
    //        {
    //            await request.Content.ReadAsStringAsync()
    //                .ContinueWith(task =>
    //                {
    //                    apiLogEntry.RequestContentBody = task.Result;
    //                }, cancellationToken);
    //        }

    //        return await base.SendAsync(request, cancellationToken)
    //            .ContinueWith(task =>
    //            {
    //                var response = task.Result;

    //                // Update the API log entry with response info
    //                apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
    //                apiLogEntry.ResponseTimestamp = DateTime.Now;

    //                if (response.Content != null)
    //                {
    //                    apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
    //                    apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
    //                    apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
    //                }

    //                // TODO: Save the API log entry to the database




    //                return response;
    //            }, cancellationToken);
    //    }

    //    private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
    //    {
    //        string user = string.Empty;
    //        string conttype = string.Empty;
    //        string userip = string.Empty;
            
    //        if (request.Properties.ContainsKey("MS_HttpContext"))
    //        {
    //            var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
    //            if (ctx != null)
    //            {
    //                user = ctx.User.Identity.Name;
    //                conttype = ctx.Request.ContentType;
    //                userip = ctx.Request.UserHostAddress;
    //            }
    //        }
            
           
    //        var routeData = request.GetRouteData();

    //        string routTemplate = string.Empty;
    //        string routData = string.Empty;

    //        if (routeData != null)
    //        {
    //            routTemplate=routeData.Route.RouteTemplate;
    //            routData = SerializeRouteData(routeData);
    //        }


    //        ApiLogEntry log =  new ApiLogEntry();
            
    //            log.Application = ConfigurationManager.AppSettings["as:AudienceId"].ToString();
    //            log.User = user;
    //            log.Machine = Environment.MachineName;
    //            log.RequestContentType = conttype;
    //            log.RequestRouteTemplate = routTemplate;
    //            log.RequestRouteData = routData;
    //            log.RequestIpAddress = userip;
    //            log.RequestMethod = request.Method.Method;
    //            log.RequestHeaders = SerializeHeaders(request.Headers);
    //            log.RequestTimestamp = DateTime.Now;
    //            log.RequestUri = request.RequestUri.ToString();
            

    //        return log;
    //    }

    //    public string GetUserIp(HttpRequestMessage request)
    //    {
    //        if (request.Properties.ContainsKey("MS_HttpContext"))
    //        {
    //            var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
    //            if (ctx != null)
    //            {
    //                return ctx.Request.UserHostAddress;
    //            }
    //        }

    //        return null;
    //    }

    //    private string SerializeRouteData(IHttpRouteData routeData)
    //    {
    //        return JsonConvert.SerializeObject(routeData, Formatting.Indented);
    //    }

    //    private string SerializeHeaders(HttpHeaders headers)
    //    {
    //        var dict = new Dictionary<string, string>();

    //        foreach (var item in headers.ToList())
    //        {
    //            if (item.Value != null)
    //            {
    //                var header = String.Empty;
    //                foreach (var value in item.Value)
    //                {
    //                    header += value + " ";
    //                }

    //                // Trim the trailing space and add item to the dictionary
    //                header = header.TrimEnd(" ".ToCharArray());
    //                dict.Add(item.Key, header);
    //            }
    //        }

    //        return JsonConvert.SerializeObject(dict, Formatting.Indented);
    //    }
    //}


    public class ApiLogHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LogRequest(request);

            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;

            LogResponse(response);

                return response;
            });
        }

        private void LogRequest(HttpRequestMessage request)
        {
             var PrincipalIdentity = request.GetOwinContext().Request.User.Identity;
            
            (request.Content ?? new StringContent("")).ReadAsStringAsync().ContinueWith(x =>
            {
                LogWriter.Log.RequestLog("HttpRequestLog", string.Format("{4:yyyy-MM-dd HH:mm:ss} {5} - {0} Request: {1}   {2} - {3}", request.GetCorrelationId(), request.Method, request.RequestUri, x.Result, DateTime.Now, Username(PrincipalIdentity)));
            }).Wait();
        }

        private void LogResponse(HttpResponseMessage response)
        {
            var PrincipalIdentity = response.RequestMessage.GetOwinContext().Request.User.Identity;

            (response.Content ?? new StringContent("")).ReadAsStringAsync().ContinueWith(x =>
            {
                LogWriter.Log.ResponseLog("HttpResponseLog", string.Format("{3:yyyy-MM-dd HH:mm:ss} {4} - {0} Response: {1}  - {2}", response.RequestMessage.GetCorrelationId(), response.StatusCode, x.Result, DateTime.Now, Username(PrincipalIdentity)));
            });
        }

        private string Username(System.Security.Principal.IIdentity identity )
        {

            if (identity != null && identity.Name != "")
            {
                return identity.Name;
            }
            else
            {
                return "anonymous";
            }

          
        }
    }
}
