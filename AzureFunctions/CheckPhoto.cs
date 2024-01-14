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

            // Extract the picture from the request
            var picture = req.Body;

            // Change the filename
            var newFilename = "Beck Alexander 20091996.jpg";





            // Convert the picture to byte[]
            using (var memoryStream = new MemoryStream())
            {
                picture.CopyTo(memoryStream);
                var pictureBytes = memoryStream.ToArray();

                // Write it to Nextcloud with the new filename
                var dlrgcloud = new DlrgCloudService("https://www.dlrg.cloud/remote.php/dav/files/albe/1904000-Leitung%20Einsatz/10-FB_Bootswesen/Test/", "albe", "r73yQ-mA55R-CxBfy-HkWXM-x2por");
                dlrgcloud.UploadBlobToDlrgCloudAsync(newFilename, pictureBytes);
            }

            // End request successfully
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
