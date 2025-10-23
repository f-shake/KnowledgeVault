using KnowledgeVault.WebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
            bool onlyPDF = config.GetValue<bool>("OnlyAcceptPDF");
            if (onlyPDF && Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                throw new StatusBasedException("仅接受PDF格式的文件", System.Net.HttpStatusCode.BadRequest);
            }
            return Ok(await fileService.UploadAsync(file));
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">成果的FileID值</param>
        /// <param name="preview">预览形式</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadAsync(string id, [FromQuery] bool preview = false)
        {
            id = Path.GetFileNameWithoutExtension(id);
            var file = await fileService.DownloadAsync(id);
            var filePath = file.DiskFilePath;

            // 根据文件类型动态设置 Content-Type
            var mimeType = GetMimeType(file.FileName);

            var stream = System.IO.File.OpenRead(filePath);
            if(preview)
            {
                return File(stream, mimeType);
            }
            return File(stream, mimeType, file.FileName);
        }

        private string GetMimeType(string filePath)
        {
            // 你可以根据文件的扩展名来设置 MIME 类型
            var extension = Path.GetExtension(filePath).ToLower();

            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".txt" => "text/plain",
                _ => "application/octet-stream",
            };
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
