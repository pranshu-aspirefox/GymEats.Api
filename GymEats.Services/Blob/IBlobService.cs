using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Blob
{
    public interface IBlobService
    {
        Task<bool> DeleteFile(string fileName, string containerName);
        Task<string> UploadFileAsync(IFormFile asset, string containerName);
        Task<string> UploadJsonAsync(string asset, string containerName);
        Task<string> DownloadJsonAsync(string filetoDownload, string azure_ContainerName);
    }
}
