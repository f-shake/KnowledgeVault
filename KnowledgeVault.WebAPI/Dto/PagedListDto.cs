namespace KnowledgeVault.WebAPI.Dto
{
    public class PagedListDto<T>
    {
        public PagedListDto()
        {
            Items = new List<T>();
        }

        public PagedListDto(IEnumerable<T> collection, int totalCount)
        {
            TotalCount = totalCount;
            Items = new List<T>(collection);
        }

        /// <summary>
        /// 查询结果
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// 不考虑筛选的总条数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 页码索引
        /// </summary>
        public int PageIndex { get; internal set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; internal set; }
    }
}
