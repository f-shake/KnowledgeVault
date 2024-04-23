using KnowledgeVault.WebAPI.Entity;

namespace KnowledgeVault.WebAPI.Dto
{
    public class ImportResultDto
    {
        /// <summary>
        /// 导入成功的文件名
        /// </summary>
        public IList<string> SucceedFiles { get; set; }

        /// <summary>
        /// 导入失败的文件名
        /// </summary>
        public IDictionary<string, string> FailedFiles { get; set; }

        /// <summary>
        /// 导入成功的成果
        /// </summary>
        public IList<AchievementEntity> ImportedAchievements { get; set; }
    }
}
