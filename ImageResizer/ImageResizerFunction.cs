using System;
using System.Collections.Generic;
using System.IO;
using Azure.Storage.Blobs.Models;
using ImageResizer.Entities;
using ImageResizer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageResizer
{
    public class ImageResizerFunction
    {
        private readonly IImageResizerService _imageResizerService;
        private readonly IImageStorageService _imageStorageService;

        public ImageResizerFunction()
        {
            _imageResizerService = new ImageResizerService();
            _imageStorageService = new ImageBlobStorageService();
        }

        [Function("ImageResizer")]
        public async void Run([CosmosDBTrigger(
            databaseName: "crud-db",
            collectionName: "Image",
            ConnectionStringSetting = "CosmosDB_ConnectionString",
            LeaseCollectionName = "Lease")] IReadOnlyList<Image> input, FunctionContext context)
        {
            var logger = context.GetLogger("ImageResizer");

            if (input != null && input.Count > 0)
            {
                logger.LogInformation("Documents modified: " + input.Count);

                foreach (Image image in input)
                {
                    if (!string.IsNullOrWhiteSpace(image.FileName))
                    {
                        BlobDownloadInfo blob = await _imageStorageService.DownloadImage("images", image.FileName);
                        Stream resizedImage = _imageResizerService.ResizeImage(blob.Content, 100, 100);
                        resizedImage.Position = 0;
                        await _imageStorageService.UploadImage(resizedImage, "thumbnails", image.FileName);
                    }
                }
            }
        }
    }
}