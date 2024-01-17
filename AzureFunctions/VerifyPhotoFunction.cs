using System.IO;
using System.Threading.Tasks;
using AbeckDev.DLRG.ExamRegistration.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AbeckDev.DLRG.ExamRegistration.Functions
{
    public class VerifyPhotoFunction
    {
        private readonly ILogger<VerifyPhotoFunction> _logger;

        public VerifyPhotoFunction(ILogger<VerifyPhotoFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(VerifyPhotoFunction))]
        public async Task Run([BlobTrigger("passphotos/{name}", Connection = "blobStorageConnectionString")] Stream stream, string name)
        {
            //Create new DLRGCloudService
            var dlrgcloud = new DlrgCloudService();
            //Detect Landesverband from Filename
            var landesverband = name.Split("-")[0];
            //Remove Landesverband from Filename
            var filename = name.Split("-")[1] + ".jpg";

            //Read the picture into a MemoryStream
            var content = new MemoryStream();
            stream.CopyTo(content);
            content.Seek(0, SeekOrigin.Begin);

            //Remove Exif Data from the picture
            var exifRemovedContent = PictureModificationService.RemoveExifData(content);

            //Do other things with the picture if needed



            //Upload the document to DLRG Cloud
            await dlrgcloud.CreateDirectory(landesverband);
            await dlrgcloud.UploadBlobToDlrgCloudAsync(landesverband + "/" + filename, exifRemovedContent, "image/jpeg");

            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} and uploaded it to {landesverband}/{filename} ");
            
            //Delete the blob from Blob Storage
            var blobService = new BlobStorageService("passphotos");
            await blobService.DeleteBlob(name);
        }
    }
}
