using System.IO;
using System.Threading.Tasks;
using AbeckDev.DLRG.ExamRegistration.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AbeckDev.DLRG.ExamRegistration.Functions
{
    public class VerifyRequirements
    {
        private readonly ILogger<VerifyRequirements> _logger;

        public VerifyRequirements(ILogger<VerifyRequirements> logger)
        {
            _logger = logger;
        }

        [Function(nameof(VerifyRequirements))]
        public async Task Run([BlobTrigger("requirements/{name}", Connection = "blobStorageConnectionString")] Stream stream, string name)
        {

            //Create new DLRGCloudService
            var dlrgcloud = new DlrgCloudService();
            //Detect Landesverband from Filename
            var landesverband = name.Split("-")[0];
            //Remove Landesverband from Filename
            var filename = name.Split("-")[1]+".pdf";

            //Read the document into a MemoryStream
            var content = new MemoryStream();
            stream.CopyTo(content);
            content.Seek(0, SeekOrigin.Begin);

            //Do things with the document if needed

            //Upload the document to DLRG Cloud
            await dlrgcloud.CreateDirectory(landesverband);
            await dlrgcloud.UploadBlobToDlrgCloudAsync(landesverband + "/" + filename,content ,"application/pdf");

            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} and uploaded it to {landesverband}/{filename} ");
        }
    }
}
