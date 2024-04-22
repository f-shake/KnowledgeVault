using KnowledgeVault.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeVault.WebAPI.Controllers
{
    /// <summary>
    /// 文件操作控制器
    /// </summary>
    /// <param name="config"></param>
    /// <param name="achievementService"></param>
    [ApiController]
    [Route("File")]
    public class FileController(IConfiguration config, AchievementService achievementService) : KnowledgeVaultControllerBase
    {
        private readonly IConfiguration config = config;
        private readonly AchievementService achievementService = achievementService;

        /// <summary>
        /// 上传文件。返回值包括一个id和一个extension，分别填写到实体的两个属性中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("未选择要上传的文件。");
            }

            var id = Guid.NewGuid().ToString("N");
            var extension=Path.GetExtension(file.FileName);
            var filePath = Path.Combine(config["AppConfiguration:BaseDir"], id);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                FileID=id,
                Extension=extension
            });
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">成果的FileID值</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadAsync(string id)
        {
            var filePath = Path.Combine(config["AppConfiguration:BaseDir"], id.ToString());

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("找不到指定的文件");
            }

            var fileStream = System.IO.File.OpenRead(filePath);
            var mimeType = "application/octet-stream";

            string fileName = null;
            try
            {
                fileName = await achievementService.GetFileNameAsync(id);
            }
            catch(KeyNotFoundException)
            {
                return NotFound("找不到对应的成果");
            }

            return File(fileStream, mimeType, fileName);
        }
    }

}
