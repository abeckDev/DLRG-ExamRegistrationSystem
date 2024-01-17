using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using AbeckDev.DLRG.ExamRegistration.Functions.Models;
using AbeckDev.DLRG.ExamRegistration.Functions.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;

namespace AbeckDev.DLRG.ExamRegistration.Functions
{
    public class ClassifyInputFunction
    {
        private readonly ILogger _logger;

        public ClassifyInputFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ClassifyInputFunction>();
        }

        [Function("ClassifyInputFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //Get all the needed registration information
            var examRegistrationRequest = new ExamRegistrationRequest(req.Query);

            //Extract Multiform Boundary from Header
            req.Headers.TryGetValues("Content-Type", out var contentType);
            string MultiformBoundary = contentType.FirstOrDefault().Split("boundary=")[1];

            var reader = new MultipartReader(MultiformBoundary, req.Body);

            var section = reader.ReadNextSectionAsync().Result;
            while (section != null)
            {
                // Process each multipart section here, the body inside each section
                // is the content you're seeking
                Regex regex = new Regex(@"name=""(.*?)"";");
                Match match = regex.Match(section.ContentDisposition);
                if (match.Success)
                {
                    //Set the filename for the picture and the requirements
                    var newFilename = examRegistrationRequest.Landesverband + "-" + examRegistrationRequest.Surname + ", " + examRegistrationRequest.Name + " " + examRegistrationRequest.Birthday.ToString("yyyy-MM-dd");

                    //Check if the current section is the picture or the requirements and upload it to Blob Storage
                    if (match.Groups[1].Value == "picture")
                    {
                        //Read the picture into a MemoryStream
                        var picture = new MemoryStream();
                        section.Body.CopyTo(picture);
                        picture.Seek(0, SeekOrigin.Begin);

                        //Upload the picture to Blob Storage
                        var blobService = new BlobStorageService("passphotos");
                        await blobService.UploadToBlobStorage(picture, newFilename + ".jpg");

                    }

                    //Check if the current section is the picture or the requirements and upload it to Blob Storage
                    if (match.Groups[1].Value == "requirements")
                    {
                        //Read the requirements into a MemoryStream
                        var document = new MemoryStream();
                        section.Body.CopyTo(document);
                        document.Seek(0, SeekOrigin.Begin);

                        //Upload the requirements to Blob Storage
                        await new BlobStorageService("requirements").UploadToBlobStorage(document, newFilename + ".pdf");
                    }
                }
                //Read the next section
                section = reader.ReadNextSectionAsync().Result;
            }

            // End request successfully
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
