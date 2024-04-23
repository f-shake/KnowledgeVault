using System.ComponentModel.DataAnnotations;

namespace KnowledgeVault.WebAPI.Entity
{
    public class EntityBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}