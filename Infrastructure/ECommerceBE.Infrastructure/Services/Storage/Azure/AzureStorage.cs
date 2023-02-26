using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerceBE.Application.Abstraction.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBE.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainer)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            //_blobContainerClient' a karşılık nesne elde edildi.
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            //ilgili container var mı yok mu? eğer yoksa ilgili container oluşturulur.
            await _blobContainerClient.CreateIfNotExistsAsync();

            //BlobContainer'a erişim izni vdrildi.
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainer)> datas = new();
            foreach (var file in files)
            {
                string fileNewName = await FileRenameAsync(containerName, file.Name, HasFile);

                //hangi blob üzeridne işlem yapılacağı bildirildi.
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);

                //IFormFile'ı OpenReadStream ile streame dönüştürdük.
                //Upload ile Azure'a gönderildi.
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{containerName}\\{fileNewName}"));
            }
            return datas;
        }
    }
}
