using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit;
using System.Web.Http;
using System.Net.Http;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using JMM.APEC.BusinessObjects.Entities;
using System.Threading;

namespace JMM.APEC.WebAPI.Tests.IntegrationTests
{
    [TestFixture]
    public class PortalControllerIntegrationTest
    {
       [Test]
        public void PortalTest()
        {
            string baseAddress = "http://localhost:50387/";

            // Server
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default", "api/v1/{controller}/{id}",
                                        new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            HttpServer server = new HttpServer(config);

            // Client
            HttpMessageInvoker messageInvoker = new HttpMessageInvoker(new InMemoryHttpContentSerializationHandler(server));

            //order to be created
           Asset_Portal portal = new Asset_Portal() {  };

            HttpRequestMessage request = new HttpRequestMessage();
            request.Content = new ObjectContent<Asset_Portal>(portal, new JsonMediaTypeFormatter());
            request.RequestUri = new Uri(baseAddress + "api/v1/Portals/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = HttpMethod.Get;

            CancellationTokenSource cts = new CancellationTokenSource();

            using (HttpResponseMessage response = messageInvoker.SendAsync(request, cts.Token).Result)
            {
                Assert.NotNull(response.Content);
                Assert.NotNull(response.Content.Headers.ContentType);
                Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
                //Assert.AreSame("jmmapec.com", response.Content.ReadAsAsync<Asset_Portal>().Result.DomainUrl);
            }
        }
    }


    public class InMemoryHttpContentSerializationHandler : DelegatingHandler
    {
        public InMemoryHttpContentSerializationHandler()
        {
        }

        public InMemoryHttpContentSerializationHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Replace the original content with a StreamContent before the request
            // passes through upper layers in the stack
            request.Content = ConvertToStreamContent(request.Content);

            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((responseTask) =>
            {
                HttpResponseMessage response = responseTask.Result;

                // Replace the original content with a StreamContent before the response
                // passes through lower layers in the stack
                response.Content = ConvertToStreamContent(response.Content);

                return response;
            });
        }

        private StreamContent ConvertToStreamContent(HttpContent originalContent)
        {
            if (originalContent == null)
            {
                return null;
            }

            StreamContent streamContent = originalContent as StreamContent;

            if (streamContent != null)
            {
                return streamContent;
            }

            MemoryStream ms = new MemoryStream();

            // **** NOTE: ideally you should NOT be doing calling Wait() as its going to block this thread ****
            // if the original content is an ObjectContent, then this particular CopyToAsync() call would cause the MediaTypeFormatters to 
            // take part in Serialization of the ObjectContent and the result of this serialization is stored in the provided target memory stream.
            originalContent.CopyToAsync(ms).Wait();

            // Reset the stream position back to 0 as in the previous CopyToAsync() call,
            // a formatter for example, could have made the position to be at the end after serialization
            ms.Position = 0;

            streamContent = new StreamContent(ms);

            // copy headers from the original content
            foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers)
            {
                streamContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return streamContent;
        }
    }
    }
