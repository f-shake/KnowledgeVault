using KnowledgeVault.Core.Dto;
using KnowledgeVault.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeVault.Core.Service
{
    public class AchievementService(KnowledgeVaultDbContext db)
    {
        private static readonly Dictionary<string, Func<IQueryable<AchievementEntity>, IOrderedQueryable<AchievementEntity>>> sortMap = new Dictionary<string, Func<IQueryable<AchievementEntity>, IOrderedQueryable<AchievementEntity>>>
        {
            { nameof(AchievementEntity.Year), query => query.OrderBy(p => p.Year) },
            { nameof(AchievementEntity.FirstAuthor), query => query.OrderBy(p => p.FirstAuthor) },
            { nameof(AchievementEntity.Correspond), query => query.OrderBy(p => p.Correspond) },
            { nameof(AchievementEntity.Type), query => query.OrderBy(p => p.Type) },
            { nameof(AchievementEntity.Theme), query => query.OrderBy(p => p.Theme) }
        };


        public async Task<string> GetFileNameAsync(string fileID)
        {
            var entity = await db.Achievements.Where(p => p.FileID == fileID).FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
            return $"{entity.FirstAuthor} {entity.Title}{entity.FileExtension}";
        }
        public async Task<PagedListDto<AchievementEntity>> GetAllAsync(PagedListRequestDto request)
        {
            IQueryable<AchievementEntity> query = db.Achievements;

            // 应用分页
            if (request.PageIndex >= 0 && request.PageSize > 0)
            {
                query = query.Skip((request.PageIndex - 1) * request.PageSize)
                             .Take(request.PageSize);
            }
            if (request.Year.HasValue)
            {
                query = query.Where(p => p.Year == request.Year);
            }
            if (!string.IsNullOrEmpty(request.Author))
            {
                query = query.Where(p => p.FirstAuthor == request.Author || p.AllAuthors.Contains(request.Author));
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

            // 应用过滤条件
            //if (request.Filters != null && request.Filters.Count > 0)
            //{
            //    foreach (var filter in request.Filters)
            //    {
            //        if (filterMap.TryGetValue(filter.Key, out var filterFunc))
            //        {
            //            query = filterFunc(query, filter.Value);
            //        }
            //    }
            //}

            // 应用排序
            if (request.SortField != null && sortMap.TryGetValue(request.SortField, out var sortFunc))
            {
                query = sortFunc(query);
            }

            // 将查询结果转换为 PagedListDto 并返回
            var pagedListDto = new PagedListDto<AchievementEntity>
            {
                Items = await query.ToListAsync(),
                TotalCount = await query.CountAsync(),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagedListDto;
        }

        public async Task<int> InsertAsync(AchievementEntity achievement)
        {
            bool isDuplicateName = await db.Achievements
                .AnyAsync(a => a.Title == achievement.Title);

            if (isDuplicateName)
            {
                throw new InvalidOperationException("已存在相同名称的成果。");
            }

            await db.Achievements.AddAsync(achievement);
            await db.SaveChangesAsync();
            return achievement.Id;
        }

        public async Task UpdateAsync(AchievementEntity achievement)
        {
            db.Achievements.Attach(achievement);
            db.Entry(achievement).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }

}
