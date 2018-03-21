using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp3
{
    public static class GetLocaleById
    {
        [FunctionName("GetById")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Calling Locale GetById() Trigger");

            // parse query parameter
            string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
                .Value;

            var data = Locale.GenerateData().Find(l => l.Id == id);

            return id == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass an id on the query string or in the request body")
                : data == null
                    ? req.CreateResponse(HttpStatusCode.BadRequest, $"No Local found for the provided Id: {id}")
                    : req.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}
