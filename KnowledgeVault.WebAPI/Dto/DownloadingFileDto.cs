namespace KnowledgeVault.WebAPI.Dto
{
    public class DownloadingFileDto
    {
        /// <summary>
        /// 文件在硬盘中的地址
        /// </summary>
        public string DiskFilePath { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }
}
