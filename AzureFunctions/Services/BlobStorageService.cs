using Azure.Storage.Blobs;

namespace AbeckDev.DLRG.ExamRegistration.Functions.Services
{
    public class BlobStorageService
    {
        BlobServiceClient blobServiceClient;
        string containerName;
        BlobContainerClient containerClient;

        public BlobStorageService(string containerName)
        {
            blobServiceClient = new BlobServiceClient(System.Environment.GetEnvironmentVariable("blobStorageConnectionString"));
            this.containerName = containerName;

            // Create the container and return a container client object
            try
            {
                containerClient = blobServiceClient.CreateBlobContainer(containerName);
            }
            catch (Exception)
            {
                containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            }
        }
            
        public async Task UploadToBlobStorage(Stream contentStream, string blobName)
        {
            // Create a new blob in the container
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            try
            {    
                await blobClient.UploadAsync(contentStream, true);   
            }
            catch (Exception)
            {
                throw new Exception("Error while uploading to Blob Storage");
            }
            
        }

        public async Task DeleteBlob(string blobName)
        {
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
