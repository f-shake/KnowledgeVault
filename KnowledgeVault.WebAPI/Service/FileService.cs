using KnowledgeVault.WebAPI.Dto;
using KnowledgeVault.WebAPI.Entity;

namespace KnowledgeVault.WebAPI.Service
{
    public class FileService(AchievementService achievementService, IConfiguration configuration)
    {
        private readonly AchievementService achievementService = achievementService;
        private readonly IConfiguration configuration = configuration;

        public async Task<DownloadingFileDto> DownloadAsync(string id)
        {
            var filePath = Path.Combine(GetFilesDir(), id);

            if (!System.IO.File.Exists(filePath))
            {
                throw new StatusBasedException("找不到指定的文件", System.Net.HttpStatusCode.NotFound);
            }

            string fileName;
            try
            {
                fileName = GetFileName(await achievementService.GetByFileIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                throw new StatusBasedException("找不到对应的成果", System.Net.HttpStatusCode.NotFound);
            }

            return new DownloadingFileDto()
            {
                DiskFilePath = filePath,
                FileName = fileName,
            };
        }

        public async Task<UploadedFileDto> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new StatusBasedException("文件为空", System.Net.HttpStatusCode.BadRequest);
            }

            var id = Guid.NewGuid().ToString("N");
            var extension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(GetFilesDir(), id);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDto()
            {
                FileID = id,
                Extension = extension
            };
        }

        public string GetFileName(AchievementEntity achievement)
        {
            var template = configuration["ExportedFileName"];
            return template
                .Replace("{Year}", achievement.Year.ToString())
                .Replace("{FirstAuthor}", achievement.FirstAuthor ?? "")
                .Replace("{Correspond}", achievement.Correspond ?? "")
                .Replace("{Title}", achievement.Title ?? "")
                .Replace("--", "-") + achievement.FileExtension;
        }

        public string GetFilesDir()
        {
            var filesDir = configuration["FilesDir"] ?? throw new ArgumentNullException("FilesDir");
            if (!Directory.Exists(filesDir))
            {
                Directory.CreateDirectory(filesDir);
            }
            return filesDir;
        }

        public string GetTempFilesDir()
        {
            var tempFilesDir = configuration["TempFilesDir"] ?? throw new ArgumentNullException("TempFilesDir");
            if (!Directory.Exists(tempFilesDir))
            {
                Directory.CreateDirectory(tempFilesDir);
            }
            return tempFilesDir;
        }
    }
}
