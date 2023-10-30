using ChatKid.Common.Logger;
using ChatKid.GoogleServices.GoogleCloudStorage.ViewModel;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ChatKid.GoogleServices.GoogleCloudStorage
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly StorageClient storageClient;
        private readonly string bucketName;
        public CloudStorageService(IConfiguration configuration, StorageClient storageClient)
        {
            this.storageClient = storageClient;
            bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket");
        }
        public async Task<FileViewModel> UploadFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                Guid id = Guid.NewGuid();
                string extension = Path.GetExtension(file.FileName);
                string objectName = $"{id}{extension}";
                try { await storageClient.UploadObjectAsync(bucketName, objectName, null, memoryStream); }
                catch(Exception e)
                {
                    Logger<CloudStorageService>.Error(e.Message);
                }
                return new FileViewModel
                {
                    Name = objectName,
                    Url = $"https://storage.googleapis.com/{bucketName}/{objectName}"
                };
            }
            
        }
    }
}
