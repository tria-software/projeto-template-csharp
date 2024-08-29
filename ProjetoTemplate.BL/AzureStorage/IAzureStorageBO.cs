using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.AzureStorage
{
    public interface IAzureStorageBO
    {
        Task<string> Upload(IFormFile file, string fileNameSystem);
        Task<bool> Delete(string fileNameSystem);
        Task<Stream> Get(string fileNameSystem);
    }
}
