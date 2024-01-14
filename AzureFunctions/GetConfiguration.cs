using System.Net;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using AbeckDev.DLRG.ExamRegistration.Functions.Models;

namespace AbeckDev.DLRG.ExamRegistration.Functions
{
    public class GetConfiguration
    {
        private readonly ILogger _logger;

        public GetConfiguration(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetConfiguration>();
        }

        [Function("GetConfiguration")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/json; charset=utf-8");

            //Return all the possible values from enum Landesverband in json with their string value
            response.WriteString(JsonConvert.SerializeObject(System.Enum.GetNames(typeof(Landesverband))));



            return response;
        }
    }
}
