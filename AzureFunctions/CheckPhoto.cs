using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using AbeckDev.DLRG.ExamRegistration.Functions.Models;
using AbeckDev.DLRG.ExamRegistration.Functions.Services;

namespace AbeckDev.DLRG.ExamRegistration.Functions
{
    public class CheckPhotoFunction
    {
        private readonly ILogger _logger;

        public CheckPhotoFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CheckPhotoFunction>();
        }

        [Function("CheckPhotoFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //Extract query parameters from request and initialize ExamRegistrationRequest
            var examRegistrationRequest = new ExamRegistrationRequest(req.Query);
            
            //Get Secrets from Envrionment Variables and initialize DlrgCloudService
            var dlrgcloud = new DlrgCloudService(System.Environment.GetEnvironmentVariable("dlrgCloudBasePath") + examRegistrationRequest.Landesverband + "/");

            // Extract the picture from the request
            var picture = req.Body;

            // Set the filename
            var newFilename = examRegistrationRequest.Surname + ", " + examRegistrationRequest.Name + " " + examRegistrationRequest.Birthday.ToString("yyyy-MM-dd") + ".jpg";

            // Convert the picture to byte[]
            using (var memoryStream = new MemoryStream())
            {
                picture.CopyTo(memoryStream);
                var pictureBytes = memoryStream.ToArray();

                // Write it to Nextcloud with the new filename
                dlrgcloud.UploadBlobToDlrgCloudAsync(newFilename, pictureBytes);
            }

            // End request successfully
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
