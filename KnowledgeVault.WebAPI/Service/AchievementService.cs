using KnowledgeVault.WebAPI.Dto;
using KnowledgeVault.WebAPI.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeVault.WebAPI.Service
{
    public class AchievementService(KnowledgeVaultDbContext db)
    {
        private static readonly Dictionary<string, Func<IQueryable<AchievementEntity>, IOrderedQueryable<AchievementEntity>>> sortMap = new Dictionary<string, Func<IQueryable<AchievementEntity>, IOrderedQueryable<AchievementEntity>>>
        {
            { nameof(AchievementEntity.Year), query => query.OrderBy(p => p.Year) },
            { nameof(AchievementEntity.FirstAuthor), query => query.OrderBy(p => p.FirstAuthor) },
            { nameof(AchievementEntity.Correspond), query => query.OrderBy(p => p.Correspond) },
            { nameof(AchievementEntity.Type), query => query.OrderBy(p => p.Type) },
            { nameof(AchievementEntity.SubType), query => query.OrderBy(p => p.SubType) },
            { nameof(AchievementEntity.Title), query => query.OrderBy(p => p.Title) },
            { nameof(AchievementEntity.Theme), query => query.OrderBy(p => p.Theme) }
        };


        public async Task<string> GetFileNameAsync(string fileID)
        {
            var entity = await db.Achievements.Where(p => p.FileID == fileID).FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
            return $"{entity.FirstAuthor} {entity.Title}{entity.FileExtension}";
        }
        public async Task<PagedListDto<AchievementEntity>> GetAllAsync(PagedListRequestDto request)
        {
            IQueryable<AchievementEntity> query = db.Achievements.EnsureNotDeleted();

            //筛选
            if (request.Year.HasValue)
            {
                query = query.Where(p => p.Year == request.Year);
            }
            if (!string.IsNullOrEmpty(request.Author))
            {
                query = query.Where(p => p.FirstAuthor.Contains(request.Author));
            }
            if (!string.IsNullOrEmpty(request.Correspond))
            {
                query = query.Where(p => p.Correspond == request.Correspond);
            }
            if (request.Type.HasValue)
            {
                query = query.Where(p => p.Type == request.Type);
            }
            if (!string.IsNullOrEmpty(request.Theme))
            {
                query = query.Where(p => p.Theme == request.Theme);
            }
            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(p => p.Title.Contains(request.Title));
            }
            if (!string.IsNullOrEmpty(request.SubType))
            {
                query = query.Where(p => p.SubType == request.SubType);
            }

            var totalCount = await query.CountAsync();

            // 分页
            if (request.PageIndex >= 0 && request.PageSize > 0)
            {
                query = query.Skip((request.PageIndex - 1) * request.PageSize)
                             .Take(request.PageSize);
            }

            // 排序
            if (request.SortField != null && sortMap.TryGetValue(request.SortField, out var sortFunc))
            {
                query = sortFunc(query);
            }

            // 将查询结果转换为 PagedListDto 并返回
            var pagedListDto = new PagedListDto<AchievementEntity>
            {
                Items = await query.ToListAsync(),
                TotalCount = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagedListDto;
        }

        public async Task<int> InsertAsync(AchievementEntity achievement)
        {
            bool isDuplicateName = await db.Achievements.EnsureNotDeleted()
                .AnyAsync(a => a.Title == achievement.Title && a.Type == achievement.Type);

            if (isDuplicateName)
            {
                throw new StatusBasedException("已存在相同名称的成果。", System.Net.HttpStatusCode.Conflict);
            }

            await db.Achievements.AddAsync(achievement);
            await db.SaveChangesAsync();
            return achievement.Id;
        }

        public async Task UpdateAsync(AchievementEntity achievement)
        {
            var existed = db.Achievements.Find(achievement.Id);
            if (existed == null || existed.IsDeleted)
            {
                throw new StatusBasedException($"找不到ID为{achievement.Id}的成果", System.Net.HttpStatusCode.NotFound);
            }
            db.Achievements.Attach(achievement);
            db.Entry(achievement).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = db.Achievements.Find(id) ?? throw new StatusBasedException($"找不到ID为{id}的成果", System.Net.HttpStatusCode.NotFound);
            entity.IsDeleted = true;
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

    }

}
