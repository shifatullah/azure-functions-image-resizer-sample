namespace ImageResizer.Services
{
    using Azure.Storage;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.Extensions.Options;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class ImageBlobStorageService : IImageStorageService
    {
        private readonly AzureStorageConfig _storageConfig = null;

        public ImageBlobStorageService()
        {
            _storageConfig = new AzureStorageConfig
            {
                Account = System.Environment.GetEnvironmentVariable("AzureStorage_Account"),
                Key = System.Environment.GetEnvironmentVariable("AzureStorage_Key"),
            };
        }

        public async Task<bool> DeleteImage(string container, string fileName)
        {
            Uri blobUri = new(GetImageUrl(container, fileName));

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.Account, _storageConfig.Key);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            // Upload the file
            await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);

            return await Task.FromResult(true);
        }

        public string GetImageUrl(string container, string fileName)
        {
            return $"https://{_storageConfig.Account}.blob.core.windows.net/{container}/{fileName}";
        }

        public async Task<bool> UploadImage(Stream fileStream, string container, string fileName)
        {
            Uri blobUri = new(GetImageUrl(container, fileName));

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.Account, _storageConfig.Key);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            // Upload the file
            await blobClient.UploadAsync(fileStream);

            return await Task.FromResult(true);
        }

        public async Task<BlobDownloadInfo> DownloadImage(string container, string fileName)
        {
            Uri blobUri = new(GetImageUrl(container, fileName));

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.Account, _storageConfig.Key);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            // Upload the file
            return await blobClient.DownloadAsync();
        }
    }
}