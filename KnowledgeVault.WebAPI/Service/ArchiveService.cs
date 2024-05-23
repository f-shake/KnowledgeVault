using CsvHelper;
using CsvHelper.Configuration;
using KnowledgeVault.WebAPI.Dto;
using KnowledgeVault.WebAPI.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace KnowledgeVault.WebAPI.Service
{
    public class ArchiveService(AppDbContext db, AchievementService achievementService, FileService fileService, IConfiguration configuration)
    {
        private readonly AchievementService achievementService = achievementService;
        private readonly IConfiguration configuration = configuration;
        private readonly AppDbContext db = db;
        public async Task<byte[]> ExportTableAsync(PagedListRequestDto request)
        {
            request.PageSize = 0;
            var data = await achievementService.GetAllAsync(request);
            using var ms = new MemoryStream();
            using TextWriter textWriter = new StreamWriter(ms, new UTF8Encoding(true));
            using var csv = new CsvWriter(textWriter, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<AchievementMap>();
            csv.WriteRecords(data.Items);
            return ms.ToArray();
        }

        public async Task<string> MakeBackupAsync()
        {
            var tempDir = fileService.GetTempFilesDir();
            var filesDir = fileService.GetFilesDir();
            var tempDb = Path.Combine(tempDir, "db.sqlite");
            if (File.Exists(tempDb))
            {
                File.Delete(tempDb);
            }
            await using (var source = new SqliteConnection(configuration.GetConnectionString("Default")))
            {
                await using var destination = new SqliteConnection($"Data Source={tempDb}");
                await source.OpenAsync();
                await destination.OpenAsync();
                source.BackupDatabase(destination);
            }
            SqliteConnection.ClearAllPools(); //不然文件锁还在


            var tempZipName = Path.Combine(tempDir, DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".zip");

            using var fs = File.Create(tempZipName);
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create, true))
            {
                zip.CreateEntryFromFile(tempDb, "db.sqlite");
                foreach (var file in Directory.EnumerateFiles(filesDir))
                {
                    zip.CreateEntryFromFile(file, Path.GetFileName(file));
                }
            }
            await fs.DisposeAsync();

            return tempZipName;
        }

        public async Task<string> ExportFilesAsync(PagedListRequestDto request)
        {
            var tempDir = fileService.GetTempFilesDir();
            var tempZipName = Path.Combine(tempDir, Guid.NewGuid().ToString("N") + ".zip");
            var achievements = (await achievementService.GetAllAsync(request)).Items;
            using var fs = File.Create(tempZipName);
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create, true))
            {
                foreach (var a in achievements)
                {
                    if (a.FileID == null)
                    {
                        continue;
                    }
                    var diskFileName = Path.Combine(fileService.GetFilesDir(), a.FileID);
                    if (!File.Exists(diskFileName))
                    {
                        continue;
                    }
                    string zipFileName = fileService.GetFileName(a);
                    zip.CreateEntryFromFile(diskFileName, zipFileName);
                }
            }
            await fs.DisposeAsync();
            return tempZipName;
        }

        public async Task<ImportResultDto> ImportAllAsync(IFormFile zipFile)
        {
            if (zipFile == null || zipFile.Length == 0)
            {
                throw new StatusBasedException("ZIP 文件为空", System.Net.HttpStatusCode.BadRequest);
            }

            var result = new ImportResultDto
            {
                SucceedFiles = new List<string>(),
                FailedFiles = new Dictionary<string, string>(),
                ImportedAchievements = new List<AchievementEntity>()
            };

            var tempDir = Path.Combine(fileService.GetFilesDir(), "temp_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                var zipPath = Path.Combine(tempDir, zipFile.FileName);
                using (var stream = new FileStream(zipPath, FileMode.Create))
                {
                    await zipFile.CopyToAsync(stream);
                }

                ZipFile.ExtractToDirectory(zipPath, tempDir, Encoding.GetEncoding("GBK"));

                File.Delete(zipPath);
                var files = Directory.EnumerateFiles(tempDir);

                foreach (var file in files)
                {
                    using var fs = new FileStream(file, FileMode.Open);
                    var fileName = Path.GetFileName(file);
                    var formFile = new FormFile(fs, 0, fs.Length, file, fileName);

                    try
                    {
                        // 调用 ImportAsync 处理每个 PDF 文件
                        var achievement = await ImportAsync(formFile);
                        result.ImportedAchievements.Add(achievement);
                        result.SucceedFiles.Add(fileName);
                    }
                    catch (Exception ex)
                    {
                        result.FailedFiles.TryAdd(fileName, ex.Message);
                    }
                }

                return result;
            }
            finally
            {
                // 删除临时目录及其内容
                Directory.Delete(tempDir, true);
            }
        }

        public async Task<AchievementEntity> ImportAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new StatusBasedException("文件为空", System.Net.HttpStatusCode.BadRequest);
            }

            string[] parts = Path.GetFileNameWithoutExtension(file.FileName).Split('$', StringSplitOptions.TrimEntries);
            AchievementEntity achievement = new AchievementEntity();

            if (parts.Length < 3)
            {
                throw new StatusBasedException("文件名格式不正确，无法解析成果类型", System.Net.HttpStatusCode.BadRequest);
            }

            // 解析年份
            int year = ParseYear(parts[1]);
            achievement.Year = year;
            achievement.FirstAuthor = parts[2];
            achievement.Title = parts[^1];

            switch (parts[0])
            {
                case "学位论文":
                case "期刊论文":
                    if (parts.Length != 6)
                    {
                        throw new StatusBasedException("文件名格式不正确，无法解析成果详细信息", System.Net.HttpStatusCode.BadRequest);
                    }
                    achievement.Type = 1;
                    achievement.SubType = parts[0];
                    achievement.Correspond = parts[3];
                    achievement.Journal = parts[4];
                    break;

                case "发明专利":
                case "实用新型专利":
                    if (parts.Length != 5)
                    {
                        throw new StatusBasedException("文件名格式不正确，无法解析成果详细信息", System.Net.HttpStatusCode.BadRequest);
                    }
                    achievement.Type = 2;
                    achievement.SubType = parts[0];
                    achievement.Number = parts[3];
                    break;

                case "软著":
                    if (parts.Length != 5)
                    {
                        throw new StatusBasedException("文件名格式不正确，无法解析成果详细信息", System.Net.HttpStatusCode.BadRequest);
                    }
                    achievement.Type = 3;
                    achievement.Number = parts[3];
                    break;

                case "奖项":
                    if (parts.Length != 4)
                    {
                        throw new StatusBasedException("文件名格式不正确，无法解析成果详细信息", System.Net.HttpStatusCode.BadRequest);
                    }
                    achievement.Type = 4;
                    break;

                default:
                    throw new StatusBasedException("未知的成果类型：" + parts[0], System.Net.HttpStatusCode.BadRequest);
            }

            if (db.Achievements.Any(p => p.Year == achievement.Year && p.Title == achievement.Title && p.FirstAuthor == achievement.FirstAuthor))
            {
                throw new StatusBasedException("已存在相同成果", System.Net.HttpStatusCode.Conflict);
            }

            var uploadDto = await fileService.UploadAsync(file);

            achievement.FileID = uploadDto.FileID;
            achievement.FileExtension = uploadDto.Extension;

            achievement.CreateTime = DateTime.Now;
            achievement.ModifiedTime = DateTime.Now;
            db.Achievements.Add(achievement);
            await db.SaveChangesAsync();
            return achievement;



            // 解析年份的方法
            static int ParseYear(string yearString)
            {
                if (int.TryParse(yearString, out int year))
                {
                    return year;
                }
                else
                {
                    throw new StatusBasedException("年份格式不正确", System.Net.HttpStatusCode.BadRequest);
                }
            }
        }
        public class AchievementMap : ClassMap<AchievementEntity>
        {
            private static readonly Dictionary<int, string> typeNames = new Dictionary<int, string>()
            {
                [0] = "未知",
                [1] = "论文",
                [2] = "专利",
                [3] = "软著",
                [4] = "奖项",
                [5] = "项目"
            };
            public AchievementMap()
            {
                Map(m => m.Title).Name("名称");
                Map(m => m.Year).Name("年份");
                Map(m => m.Type).Name("类型").Convert(p => typeNames[p.Value.Type]);
                Map(m => m.SubType).Name("子类型");
                Map(m => m.FirstAuthor).Name("学生");
                Map(m => m.FirstAuthor).Name("老师");
                Map(m => m.AllAuthors).Name("所有作者");
                Map(m => m.Journal).Name("期刊名");
                Map(m => m.Number).Name("号码");
                Map(m => m.Amount).Name("金额");
            }
        }

    }

}
