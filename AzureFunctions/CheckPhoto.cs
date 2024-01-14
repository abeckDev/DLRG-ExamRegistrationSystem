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

            //save query parameters to string variables
            var name = req.Query["name"];
            var surname = req.Query["surname"];
            //save birthday from query parameters to DateTime variable
            var birthday = System.DateTime.Parse(req.Query["birthday"]);
            //Save Landesverband from query parameters to Landesverband variable as enum Landesverband
            var landesverband = (Landesverband)System.Enum.Parse(typeof(Landesverband), req.Query["landesverband"]);

            // Extract the picture from the request
            var picture = req.Body;

            // Change the filename
            var newFilename = surname + ", " + name + " " + birthday.ToString("yyyy-MM-dd") + ".jpg";

            // Convert the picture to byte[]
            using (var memoryStream = new MemoryStream())
            {
                picture.CopyTo(memoryStream);
                var pictureBytes = memoryStream.ToArray();

                // Write it to Nextcloud with the new filename
                var dlrgcloud = new DlrgCloudService("https://www.dlrg.cloud/remote.php/dav/files/albe/1904000-Leitung%20Einsatz/10-FB_Bootswesen/Test/" + landesverband + "/", "albe", "r73yQ-mA55R-CxBfy-HkWXM-x2por");

                dlrgcloud.UploadBlobToDlrgCloudAsync(newFilename, pictureBytes);
            }

            // End request successfully
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
