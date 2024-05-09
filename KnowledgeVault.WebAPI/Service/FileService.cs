using KnowledgeVault.WebAPI.Dto;

namespace KnowledgeVault.WebAPI.Service
{
    public class FileService(AchievementService achievementService)
    {
        private readonly AchievementService achievementService = achievementService;

        public async Task<DownloadingFileDto> DownloadAsync(string id, string baseDir)
        {
            var filePath = Path.Combine(baseDir, id);

            if (!System.IO.File.Exists(filePath))
            {
                throw new StatusBasedException("找不到指定的文件", System.Net.HttpStatusCode.NotFound);
            }

            string fileName;
            try
            {
                fileName = await achievementService.GetFileNameAsync(id);
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

        public async Task<UploadedFileDto> UploadAsync(IFormFile file, string baseDir)
        {
            if (file == null || file.Length == 0)
            {
                throw new StatusBasedException("文件为空", System.Net.HttpStatusCode.BadRequest);
            }

            var id = Guid.NewGuid().ToString("N");
            var extension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(baseDir, id);

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
    }
}
