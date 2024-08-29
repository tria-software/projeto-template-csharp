using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using ProjetoTemplate.Domain.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.AzureStorage
{

    public class AzureStorageBO : IAzureStorageBO
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        public AzureStorageBO(AzureConfig azureConfig, BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _containerClient = _blobServiceClient.GetBlobContainerClient(azureConfig.ContainerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> Upload(IFormFile file, string fileNameSystem)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(fileNameSystem);
                var fileblob = await blobClient.UploadAsync(file.OpenReadStream(), true);

                // Configura o cabeçalho Content - Disposition para definir o nome do arquivo no download
                var httpHeaders = new BlobHttpHeaders
                {
                    ContentDisposition = $"attachment; filename={file.FileName}"
                };

                await blobClient.SetHttpHeadersAsync(httpHeaders);

                var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTime.UtcNow.AddYears(99));

                return sasUri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<bool> Delete(string fileNameSystem)
        {
            try
            {
                var blob = _containerClient.GetBlobClient(fileNameSystem);

                await blob.DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Stream> Get(string fileNameSystem)
        {
            try
            {
                var blob = _containerClient.GetBlobClient(fileNameSystem);

                var response = await blob.DownloadStreamingAsync();
                return response.Value.Content;
            }
            catch
            {
                return null;
            }
        }
    }
}
