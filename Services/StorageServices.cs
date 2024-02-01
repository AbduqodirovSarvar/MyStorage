using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Any;

namespace MyStorage.Services
{
    public class StorageServices
    {
        private readonly IWebHostEnvironment _env;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;
        public StorageServices(IWebHostEnvironment env)
        {
            _env = env;
            _contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        public async Task<Uri> Save(IFormFile file, CancellationToken cancellationToken)
        {
            string webRootPath = _env.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(webRootPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream, cancellationToken);

            var fullUrl = "https://storage.sarvarbekabduqodirov.uz:8443/" + "api/" + fileName;
            return new Uri(fullUrl);
        }

        public (string, string) Get(string fileName)
        {
            string webRootPath = _env.WebRootPath;
            var filePath = Path.Combine(webRootPath, fileName);
            if (_contentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                return (filePath, contentType);
            }

            return (filePath, "application/octet-stream");
        }
    }
}
