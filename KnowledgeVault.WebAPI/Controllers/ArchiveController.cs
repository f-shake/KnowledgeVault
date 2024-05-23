using KnowledgeVault.WebAPI.Dto;
using KnowledgeVault.WebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace KnowledgeVault.WebAPI.Controllers
{
    /// <summary>
    /// 导入导出控制器
    /// </summary>
    /// <param name="config"></param>
    /// <param name="fileService"></param>
    [ApiController]
    [Route("Archive")]
    public class ArchiveController(IConfiguration config,
                                   ArchiveService archiveService,
                                   FileService fileService,
                                   AchievementService achievementService) : KnowledgeVaultControllerBase
    {
        private readonly ArchiveService archiveService = archiveService;
        private readonly FileService fileService = fileService;
        private readonly AchievementService achievementService = achievementService;

        /// <summary>
        /// 导出请求的成果的文件压缩包
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ExportFiles")]
        public async Task<IActionResult> ExportFilesAsync([FromQuery] PagedListRequestDto request)
        {
            if (isArchiving)
            {
                throw new StatusBasedException("存在正在执行的打包任务", System.Net.HttpStatusCode.BadRequest);
            }
            isArchiving = true;
            try
            {
                var file = await archiveService.ExportFilesAsync(request);
                var mimeType = "application/zip";
                return PhysicalFile(file, mimeType, "成果文件.zip");
            }
            finally
            {
                isArchiving = false;
            }
        }

        /// <summary>
        /// 导出所有数据用于备份
        /// </summary>
        /// <returns></returns>
        /// <exception cref="StatusBasedException"></exception>
        [HttpGet]
        [Route("DownloadLastBackup")]
        public async Task<IActionResult> DownloadLastBackup()
        {
            if (isArchiving)
            {
                throw new StatusBasedException("存在正在执行的打包任务", System.Net.HttpStatusCode.BadRequest);
            }
            isArchiving = true;
            try
            {
                var tempDir = fileService.GetTempFilesDir();
                var lastFile = Directory.EnumerateFiles(tempDir, "*.zip")
                    .OrderByDescending(p => p)
                    .FirstOrDefault()
                    ?? throw new StatusBasedException("不存在任何备份文件", System.Net.HttpStatusCode.NotFound);
                var mimeType = "application/zip";
                var file = System.IO.File.OpenRead(lastFile);
                return File(file, mimeType, "成果备份.zip");
            }
            finally
            {
                isArchiving = false;
            }
        }

        /// <summary>
        /// 制作一个备份文件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="StatusBasedException"></exception>
        [HttpPost]
        [Route("MakeBackup")]
        public async Task<IActionResult> MakeBackupAsync()
        {
            if (isArchiving)
            {
                throw new StatusBasedException("存在正在执行的打包任务", System.Net.HttpStatusCode.BadRequest);
            }
            isArchiving = true;
            try
            {
                var file = await archiveService.MakeBackupAsync();
                return Ok(Path.GetFileName(file));
            }
            finally
            {
                isArchiving = false;
            }
        }

        private static bool isArchiving = false;

        /// <summary>
        /// 导出请求的成果的汇总表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ExportTable")]
        public async Task<IActionResult> ExportTableAsync([FromQuery] PagedListRequestDto request)
        {
            var mimeType = "application/octet-stream";
            var data = await archiveService.ExportTableAsync(request);
            return File(data, mimeType, "成果表.csv");
        }

        /// <summary>
        /// 批量上传文件，所有文件放置在一个ZIP中，并根据文件名自动插入成果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportAll")]
        public async Task<IActionResult> ImportAllAsync(IFormFile file)
        {
            return Ok(await archiveService.ImportAllAsync(file));
        }

        /// <summary>
        /// 上传文件，并根据文件名自动插入成果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Import")]
        public async Task<IActionResult> ImportAsync(IFormFile file)
        {
            return Ok(await archiveService.ImportAsync(file));
        }
    }

}
