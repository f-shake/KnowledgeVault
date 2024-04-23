namespace KnowledgeVault.WebAPI.Entity
{
    public static class EntityExtension
    {
        public static IQueryable<T> EnsureNotDeleted<T>(this IQueryable<T> query) where T : EntityBase
        {
            return query.Where(p => p.IsDeleted == false);
        }
    }
}