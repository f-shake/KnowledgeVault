using KnowledgeVault.WebAPI.Dto;
using KnowledgeVault.WebAPI.Entity;
using Mapster;
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
        public async Task<string> GetFileNameAsync(string fileID)
        {
            var entity = await db.Achievements.Where(p => p.FileID == fileID).FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
            return $"{entity.FirstAuthor} {entity.Title}{entity.FileExtension}";
        }

        public async Task<AchievementEntity> GetAsync(int id)
        {
            var entity = db.Achievements.Find(id) ?? throw new StatusBasedException($"找不到ID为{id}的成果", System.Net.HttpStatusCode.NotFound);
            if (entity.IsDeleted)
            {
                throw new StatusBasedException($"ID为{id}的成果已删除", System.Net.HttpStatusCode.NotFound);
            }
            return entity;
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
                query = query.Where(p => p.Correspond.Contains(request.Correspond));
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
                query = query.Where(p => p.Title.ToLower().Contains(request.Title.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.SubType))
            {
                query = query.Where(p => p.SubType == request.SubType);
            }

            //确认总数
            var totalCount = await query.CountAsync();

            // 排序
            Dictionary<string, Func<IQueryable<AchievementEntity>, IOrderedQueryable<AchievementEntity>>> sortMap;
            if (request.SortOrder)
            {
                sortMap = new Dictionary<string, Func<IQueryable<AchievementEntity>, IOrderedQueryable<AchievementEntity>>>
                {
                    { nameof(AchievementEntity.CreateTime).ToLower(), query => query.OrderBy(p => p.CreateTime) },
                    { nameof(AchievementEntity.ModifiedTime).ToLower(), query => query.OrderBy(p => p.ModifiedTime) },
                    { nameof(AchievementEntity.Year).ToLower(), query => query.OrderBy(p => p.Year) },
                    { nameof(AchievementEntity.FirstAuthor).ToLower(), query => query.OrderBy(p => p.FirstAuthor) },
                    { nameof(AchievementEntity.Correspond).ToLower(), query => query.OrderBy(p => p.Correspond) },
                    { nameof(AchievementEntity.Type).ToLower(), query => query.OrderBy(p => p.Type) },
                    { nameof(AchievementEntity.SubType).ToLower(), query => query.OrderBy(p => p.SubType) },
                    { nameof(AchievementEntity.Title).ToLower(), query => query.OrderBy(p => p.Title) },
                    { nameof(AchievementEntity.Theme).ToLower(), query => query.OrderBy(p => p.Theme) },
                    { nameof(AchievementEntity.Number).ToLower(), query => query.OrderBy(p => p.Number) }
                };
            }
            else
            {
                sortMap = new Dictionary<string, Func<IQueryable<AchievementEntity>, IOrderedQueryable<AchievementEntity>>>
                {
                    { nameof(AchievementEntity.CreateTime).ToLower(), query => query.OrderByDescending(p => p.CreateTime) },
                    { nameof(AchievementEntity.ModifiedTime).ToLower(), query => query.OrderByDescending(p => p.ModifiedTime) },
                    { nameof(AchievementEntity.Year).ToLower(), query => query.OrderByDescending(p => p.Year) },
                    { nameof(AchievementEntity.FirstAuthor).ToLower(), query => query.OrderByDescending(p => p.FirstAuthor) },
                    { nameof(AchievementEntity.Correspond).ToLower(), query => query.OrderByDescending(p => p.Correspond) },
                    { nameof(AchievementEntity.Type).ToLower(), query => query.OrderByDescending(p => p.Type) },
                    { nameof(AchievementEntity.SubType).ToLower(), query => query.OrderByDescending(p => p.SubType) },
                    { nameof(AchievementEntity.Title).ToLower(), query => query.OrderByDescending(p => p.Title) },
                    { nameof(AchievementEntity.Theme).ToLower(), query => query.OrderByDescending(p => p.Theme) },
                    { nameof(AchievementEntity.Number).ToLower(), query => query.OrderByDescending(p => p.Number) }
                };
            }
            if (request.SortField != null && sortMap.TryGetValue(request.SortField.ToLower(), out var sortFunc))
            {
                query = sortFunc(query);
            }


            // 分页
            if (request.PageIndex >= 0 && request.PageSize > 0)
            {
                query = query.Skip((request.PageIndex - 1) * request.PageSize)
                             .Take(request.PageSize);
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
            achievement.CreateTime = DateTime.Now;
            achievement.ModifiedTime = DateTime.Now;
            achievement.IsDeleted = false;
            db.Achievements.Add(achievement);
            await db.SaveChangesAsync();
            return achievement.Id;
        }

        public async Task UpdateAsync(AchievementEntity achievement)
        {
            var existed = await db.Achievements.FindAsync(achievement.Id);
            if (existed == null || existed.IsDeleted)
            {
                throw new StatusBasedException($"找不到ID为{achievement.Id}的成果", System.Net.HttpStatusCode.NotFound);
            }
            achievement.ModifiedTime = DateTime.Now;
            achievement.IsDeleted = false;
            achievement.Adapt(existed);

            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await db.Achievements.FindAsync(id) ?? throw new StatusBasedException($"找不到ID为{id}的成果", System.Net.HttpStatusCode.NotFound);
            entity.IsDeleted = true;
            entity.ModifiedTime = DateTime.Now;
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

    }

}
