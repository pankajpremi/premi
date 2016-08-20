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
using JMM.APEC.BusinessObjects.Entities;
using JMM.APEC.DataObjects;
using JMM.APEC.WebAPI.DataAccess;

namespace JMM.APEC.WebAPI
{

    public class ApiLogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        apiLogEntry.RequestContentBody = task.Result;
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    // Update the API log entry with response info
                    apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                    apiLogEntry.ResponseTimestamp = DateTime.Now;

                    if (response.Content != null)
                    {
                        apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                        apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                        apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                    }

                    // TODO: Save the API log entry to the database

                    ApecDatabase database = new ApecDatabase();
                    SystemApiLogEntryTable logtable = new SystemApiLogEntryTable(database);
                    
                    apiLogEntry.ApiLogEntryId = logtable.System_SaveApiLogEntry(apiLogEntry.Application, apiLogEntry.User, apiLogEntry.Machine, apiLogEntry.RequestIpAddress,
                    apiLogEntry.RequestContentType, apiLogEntry.RequestContentBody, apiLogEntry.RequestUri, apiLogEntry.RequestMethod,apiLogEntry.RequestRouteTemplate,
                    apiLogEntry.RequestRouteData,apiLogEntry.RequestHeaders,apiLogEntry.RequestTimestamp,apiLogEntry.ResponseContentType,apiLogEntry.ResponseContentBody,
                    apiLogEntry.ResponseStatusCode,apiLogEntry.ResponseHeaders,apiLogEntry.ResponseTimestamp);

                        

                    return response;
                }, cancellationToken);
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            string user = string.Empty;
            string conttype = string.Empty;
            string userip = string.Empty;
            
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
                if (ctx != null)
                {
                    user = ctx.User.Identity.Name;
                    conttype = ctx.Request.ContentType;
                    userip = ctx.Request.UserHostAddress;
                }
            }
            
           
            var routeData = request.GetRouteData();

            string routTemplate = string.Empty;
            string routData = string.Empty;

            if (routeData != null)
            {
                routTemplate=routeData.Route.RouteTemplate;
                routData = SerializeRouteData(routeData);
            }


            ApiLogEntry log =  new ApiLogEntry();
            
                log.Application = ConfigurationManager.AppSettings["as:AudienceId"].ToString();
                log.User = user;
                log.Machine = Environment.MachineName;
                log.RequestContentType = conttype;
                log.RequestRouteTemplate = routTemplate;
                log.RequestRouteData = routData;
                log.RequestIpAddress = userip;
                log.RequestMethod = request.Method.Method;
                log.RequestHeaders = SerializeHeaders(request.Headers);
                log.RequestTimestamp = DateTime.Now;
                log.RequestUri = request.RequestUri.ToString();
            

            return log;
        }

        public string GetUserIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            return null;
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
    //public class ApiLoggingHandler : DelegatingHandler
    //{
    //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        LogRequest(request);

    //        return base.SendAsync(request, cancellationToken).ContinueWith(task =>
    //        {
    //            var response = task.Result;

    //            LogResponse(response);

    //            return response;
    //        });
    //    }

    //    private void LogRequest(HttpRequestMessage request)
    //    {
    //        (request.Content ?? new StringContent("")).ReadAsStringAsync().ContinueWith(x =>
    //        {
    //            LogWriter.Log.RequestLog("LogRequest",string.Format("{4:yyyy-MM-dd HH:mm:ss} {5} {0} request [{1}]{2} - {3}", request.GetCorrelationId(), request.Method, request.RequestUri, x.Result, DateTime.Now, Username(request)));
    //        }).Wait(); 
    //    }

    //    private void LogResponse(HttpResponseMessage response)
    //    {
    //        var request = response.RequestMessage;
    //        (response.Content ?? new StringContent("")).ReadAsStringAsync().ContinueWith(x =>
    //        {
    //            LogWriter.Log.ResponseLog("LogResponse",string.Format("{3:yyyy-MM-dd HH:mm:ss} {4} {0} response [{1}] - {2}", request.GetCorrelationId(), response.StatusCode, x.Result, DateTime.Now, Username(request)));
    //        });
    //    }

    //    private string Username(HttpRequestMessage request)
    //    {
    //        var values = new List<string>().AsEnumerable();
    //        //if (request.Headers.TryGetValues("my-custom-header-for-current-user", out values) == false) return "<anonymous>";
    //        //if (request.Properties.ContainsKey("MS_HttpContext"))
    //        //{
    //        //    return (HttpContext)request.getRemoteUser();
    //        //}
    //        //else
    //        //{
    //        //    return "anonymous";
    //        //}

    //        //return values.First();
    //        return "anonymous";
    //    }
    //}
}
