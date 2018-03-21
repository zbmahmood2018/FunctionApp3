using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp3
{
    public static class GetAllLocale
    {
        [FunctionName("GetAll")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Calling Local GetAll Trigger");

            var list = Locale.GenerateData();

            return req.CreateResponse(HttpStatusCode.OK, list);
        }
    }
}