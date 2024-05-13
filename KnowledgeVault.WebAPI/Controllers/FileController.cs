using KnowledgeVault.WebAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeVault.WebAPI.Controllers
{
    /// <summary>
    /// 文件操作控制器
    /// </summary>
    /// <param name="config"></param>
    /// <param name="fileService"></param>
    [ApiController]
    [Route("File")]
    public class FileController(IConfiguration config, FileService fileService) : KnowledgeVaultControllerBase
    {
        private readonly IConfiguration config = config;
        private readonly FileService fileService = fileService;

        /// <summary>
        /// 上传文件。返回值包括一个id和一个extension，分别填写到实体的两个属性中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            return Ok(await fileService.UploadAsync(file, config["FilesDir"]));
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">成果的FileID值</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadAsync(string id)
        {
            var mimeType = "application/octet-stream";
            var file = await fileService.DownloadAsync(id, config["FilesDir"]);
            var stream = System.IO.File.OpenRead(file.DiskFilePath);
            return File(stream, mimeType, file.FileName);
        }
    }

}
