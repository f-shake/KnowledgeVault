using KnowledgeVault.WebAPI.Dto;
using KnowledgeVault.WebAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeVault.WebAPI.Controllers
{
    /// <summary>
    /// 导入导出控制器
    /// </summary>
    /// <param name="config"></param>
    /// <param name="fileService"></param>
    [ApiController]
    [Route("Archive")]
    public class ArchiveController(IConfiguration config, ArchiveService archiveService) : KnowledgeVaultControllerBase
    {
        /// <summary>
        /// 导出请求的成果的文件压缩包
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ExportFiles")]
        public async Task<IActionResult> ExportFilesAsync([FromQuery] PagedListRequestDto request)
        {
            return Ok("还未实现");
        }

        /// <summary>
        /// 导出请求的成果的汇总表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ExportTable")]
        public async Task<IActionResult> ExportTableAsync([FromQuery] PagedListRequestDto request)
        {
            return Ok("还未实现");
        }

        /// <summary>
        /// 批量上传文件，所有文件放置在一个ZIP中，并根据文件名自动插入成果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportAll")]
        public async Task<IActionResult> ImportAllAsync(IFormFile file)
        {
            return Ok(await archiveService.ImportAllAsync(file, config["AppConfiguration:BaseDir"]));
        }

        /// <summary>
        /// 上传文件，并根据文件名自动插入成果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Import")]
        public async Task<IActionResult> ImportAsync(IFormFile file)
        {
            return Ok(await archiveService.ImportAsync(file, config["AppConfiguration:BaseDir"]));
        }
    }

}
