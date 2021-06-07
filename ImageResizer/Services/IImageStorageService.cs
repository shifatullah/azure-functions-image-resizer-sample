using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace ImageResizer.Services
{

    public interface IImageStorageService
    {
        Task<bool> UploadImage(Stream stream, string container, string fileName);
        Task<bool> DeleteImage(string container, string fileName);

        string GetImageUrl(string container, string fileName);

        Task<BlobDownloadInfo> DownloadImage(string container, string fileName);
    }
}
