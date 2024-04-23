namespace KnowledgeVault.WebAPI.Dto
{
    public class PagedListRequestDto
    {
        /// <summary>
        /// 页码索引
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 第一作者查询（模糊）
        /// </summary>
        public string Author {  get; set; }

        /// <summary>
        /// 通讯作者查询（模糊）
        /// </summary>
        public string Correspond {  get; set; }

        /// <summary>
        /// 年份查询
        /// </summary>
        public int? Year {  get; set; }

        /// <summary>
        /// 类型查询
        /// </summary>
        public int? Type {  get; set; }

        /// <summary>
        /// 子类型查询
        /// </summary>
        public string SubType {  get; set; }

        /// <summary>
        /// 领域查询
        /// </summary>
        public string Theme {  get; set; }

        /// <summary>
        /// 标题查询（模糊）
        /// </summary>
        public string Title {  get; set; }

        /// <summary>
        /// 用于排序的字段名（与查询字段名一致）
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序顺序，true表示正序，false表示逆序
        /// </summary>
        public bool SortOrder { get; set; }
    }
}
