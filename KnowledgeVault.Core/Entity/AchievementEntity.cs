namespace KnowledgeVault.Core.Entity
{
    public class AchievementEntity : EntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 第一作者/第一负责人
        /// </summary>
        public string FirstAuthor { get; set; }

        /// <summary>
        /// 所有作者
        /// </summary>
        public string AllAuthors { get; set; }

        /// <summary>
        /// 通讯作者
        /// </summary>
        public string Correspond { get; set; }

        /// <summary>
        /// 成果发表年份，0表示未填写
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 成果类型，0未知；1论文；2专利；3软著；4奖项
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 成果子类型，如1论文分为学位论文、期刊论文；2专利分为发明专利、实用新型专利和外观设计专利等
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// 成果领域
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 对应的文件识别码。调用时，使用/File?id={FileID}
        /// </summary>
        public string FileID { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension {  get; set; }
    }
}
