using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace JMM.APEC.WebAPI
{
    public class LanguageMessageHandler : DelegatingHandler
    {
        private const string LangfrCA = "fr-CA";
        private const string LangenUS = "en-US";

        private readonly List<string> _supportedLanguages = new List<string> { LangfrCA, LangenUS };

        //private static CultureInfo DetermineBestCulture(HttpRequestMessage request)
        //{
        //    // Somehow determine the best-suited culture for the specified request,
        //    // e.g. by looking at route data, passed headers, user preferences, etc.
        //    return request.GetRouteData().Values["lang"].ToString() == "fr"
        //        ? CultureInfo.GetCultureInfo(LangfrCA)
        //        : CultureInfo.GetCultureInfo(LangenUS);
        //}


        private bool SetHeaderIfAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            foreach (var lang in request.Headers.AcceptLanguage)
            {
                if (_supportedLanguages.Contains(lang.Value))
                {
                    SetCulture(request, lang.Value);
                    return true;
                }
            }

            return false;
        }

        private bool SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            foreach (var lang in request.Headers.AcceptLanguage)
            {
                var globalLang = lang.Value.Substring(0, 2);
                if (_supportedLanguages.Any(t => t.StartsWith(globalLang)))
                {
                    SetCulture(request, _supportedLanguages.FirstOrDefault(i => i.StartsWith(globalLang)));
                    return true;
                }
            }

            return false;
        }

        private void SetCulture(HttpRequestMessage request, string lang)
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(lang));
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!SetHeaderIfAcceptLanguageMatchesSupportedLanguage(request))
            {
                // Whoops no localization found. Lets try Globalisation
                if (!SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(request))
                {
                    // no global or localization found
                    SetCulture(request, LangenUS);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}