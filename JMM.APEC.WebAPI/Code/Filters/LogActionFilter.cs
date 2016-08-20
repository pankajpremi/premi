using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

namespace JMM.APEC.WebAPI.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var userName = "Anonymous";
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                userName = actionContext.RequestContext.Principal.Identity.Name;
            }

            var reflectedActionDescriptor = actionContext.ActionDescriptor as ReflectedHttpActionDescriptor;
           

            string route = actionContext.Request.GetRouteData().Route.RouteTemplate;
            string method = actionContext.Request.Method.Method;
            string url = actionContext.Request.RequestUri.AbsoluteUri;
            var actionName = reflectedActionDescriptor.ActionName;
            var controllerName = reflectedActionDescriptor.ControllerDescriptor.ControllerName;

            var message = String.Format("Route: {0}, Controller:{1}, Action:{2}", route, controllerName, actionName);

            Logging.LogWriter.Log.LogMessage(userName, message);

        }

              
    }

}
