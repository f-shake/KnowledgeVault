using Microsoft.EntityFrameworkCore;

namespace KnowledgeVault.Core.Service
{
    public class PropertyService(KnowledgeVaultDbContext db)
    {
        public Task<List<string>> GetFirstAuthorsAsync()
        {
            return db.Achievements.Select(p => p.FirstAuthor).Distinct().ToListAsync();
        }
        public Task<List<string>> GetCorrespondingAuthorsAsync()
        {
            return db.Achievements.Select(p => p.Correspond).Distinct().ToListAsync();
        }
        public Task<List<int>> GetYearsAsync()
        {
            return db.Achievements.Select(p => p.Year).Distinct().ToListAsync();
        }
        public Task<List<string>> GetThemesAsync()
        {
            return db.Achievements.Select(p => p.Theme).Distinct().ToListAsync();
        }
    }

}
