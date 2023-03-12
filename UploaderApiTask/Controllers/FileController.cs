using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace UploaderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=testing231;AccountKey=ICnG0YqxDZog/FQNoxzwqyklAhbZtnWQWhCduObrvDQytIWKI2RQ5pfVpeLIjVGVI2VExHnSW+bO+AStZ3yQgA==;EndpointSuffix=core.windows.net";

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFileToStorage(IFormFile file, string email)
        {
            if (file == null)
            {
                return BadRequest("Please select a file to upload.");
            }

            if (!file.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("File must be a docx file.");
            }

            BlobContainerClient blobContainerClient = new BlobContainerClient(_connectionString, "files");
            string blobName = $"{email}/{file.FileName}";

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await blobContainerClient.UploadBlobAsync(blobName, stream);
            }
            //test comment

            return Ok("File uploaded successfully");
        }
    }
}