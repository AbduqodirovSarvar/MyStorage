using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyStorage.Services;

namespace MyStorage.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly StorageServices _storageServices;
        public StorageController(StorageServices storageServices)
        {
            _storageServices = storageServices;
        }
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file, CancellationToken cancellationToken = default)
        {
            return Ok(await _storageServices.Save(file, cancellationToken));
        }

        [HttpGet("{fileName}")]
        public IActionResult Get([FromRoute] string fileName)
        {
            var (filePath, contentType) = _storageServices.Get(fileName);
            return PhysicalFile(filePath, contentType);
        }
    }
}
