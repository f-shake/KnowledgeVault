using KnowledgeVault.WebAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeVault.WebAPI.Service
{
    public class PropertyService(AppDbContext db)
    {
        public Task<List<string>> GetFirstAuthorsAsync()
        {
            return db.Achievements.EnsureNotDeleted().Select(p => p.FirstAuthor).Distinct().ToListAsync();
        }
        public Task<List<string>> GetCorrespondingAuthorsAsync()
        {
            return db.Achievements.EnsureNotDeleted().Select(p => p.Correspond).Distinct().ToListAsync();
        }
        public Task<List<int>> GetYearsAsync()
        {
            return db.Achievements.EnsureNotDeleted().Select(p => p.Year).Distinct().ToListAsync();
        }
        public Task<List<string>> GetThemesAsync()
        {
            return db.Achievements.EnsureNotDeleted().Select(p => p.Theme).Distinct().ToListAsync();
        }
    }

}
