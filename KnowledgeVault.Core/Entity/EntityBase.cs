using System.ComponentModel.DataAnnotations;

namespace KnowledgeVault.Core.Entity
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}