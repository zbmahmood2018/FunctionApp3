using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System.Linq;

namespace FunctionApp3
{
    public static class GetAllLocale
    {
        private static string endPoint = ConfigurationManager.AppSettings["CosmosDB_EndPoint"];
        private static string authKey =  ConfigurationManager.AppSettings["CosmosDB_AuthKey"];
        
        private static TraceWriter _log;
        [FunctionName("GetAll")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            _log = log;

            log.Info($"Calling Trigger GetAll for Locale");

            try
            {
                var list = await ExecuteSimpleQuery("localization-zbm-test", "countrylist");
                return req.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception e)
            {
                _log.Error(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, $"Unable to Retrieve Locale: {e.Message}");
            }
        }

        private static async Task<List<Locale>> ExecuteSimpleQuery(string databaseName, string collectionName)
        {
            //Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, MaxDegreeOfParallelism = -1, };

            //Retrieve the list of Locale (Get All) with Linq
            return await (_client.CreateDocumentQuery<Locale>(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions))
                .AsDocumentQuery()
                .ToListAsync();
        }

        private static Lazy<DocumentClient> lazyClient = new Lazy<DocumentClient>(InitializeDocumentClient);
        private static DocumentClient _client => lazyClient.Value;

        private static DocumentClient InitializeDocumentClient()
        {
            var client = new DocumentClient(new Uri(endPoint), authKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
            });
            client.OpenAsync();
            return client;
        }
    }
}