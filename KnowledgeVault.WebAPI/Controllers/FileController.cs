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
            return Ok(await fileService.UploadAsync(file));
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
            var file = await fileService.DownloadAsync(id);
            var stream = System.IO.File.OpenRead(file.DiskFilePath);
            return File(stream, mimeType, file.FileName);
        }

        /// <summary>
        /// 寻找不再使用的文件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="StatusBasedException"></exception>
        [HttpGet]
        [Route("FindUnusedFiles")]
        public async Task<IActionResult> FindUnusedFiles()
        {
            var files = await fileService.FindUnusedFiles();
            return Ok(files);
        }

        /// <summary>
        /// 寻找不再使用的文件并删除
        /// </summary>
        /// <returns></returns>
        /// <exception cref="StatusBasedException"></exception>
        [HttpPost]
        [Route("DeleteUnusedFiles")]
        public async Task<IActionResult> DeleteUnusedFiles()
        {
            var files = await fileService.DeleteUnusedFiles();
            return Ok(files);
        }

    }

}
