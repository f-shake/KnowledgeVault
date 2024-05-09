namespace KnowledgeVault.WebAPI.Entity
{
    /// <summary>
    /// 成果实体
    /// </summary>
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
        /// 成果类型，0未知；1论文；2专利；3软著；4奖项；5基金项目
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 成果子类型，如1论文分为学位论文、期刊论文；2专利分为发明专利、实用新型专利和外观设计专利；5基金项目分为国家级、省部级、厅市级及以下、横向
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// 成果领域
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 期刊名
        /// </summary>
        public string Journal { get; set; }

        /// <summary>
        /// 专利号/申请号等
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Amount {  get; set; }

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
