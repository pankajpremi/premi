using JMM.APEC.ActionService;
using JMM.APEC.WebAPI.Controllers;
using System.Web.Http;

namespace JMM.APEC.WebAPI.Areas.Secure.Controllers
{
    [RoutePrefix("api/v1/sensors")]
    public class SensorController : SecureApiController
    {
        IService service { get; set; }

        public SensorController()
        {
            service = new Service(CurrentApiUser);
        }

     


    }
}
