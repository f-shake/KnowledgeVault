using KnowledgeVault.WebAPI.Dto;
using KnowledgeVault.WebAPI.Entity;
using KnowledgeVault.WebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KnowledgeVault.WebAPI.Controllers
{
    /// <summary>
    /// 成果控制器
    /// </summary>
    /// <param name="achievementService"></param>
    /// <param name="configuration"></param>
    [ApiController]
    [Route("Achievement")]
    public class AchievementController(AchievementService achievementService, IConfiguration configuration) : KnowledgeVaultControllerBase
    {
        private readonly AchievementService achievementService = achievementService;
        private readonly IConfiguration configuration = configuration;

        /// <summary>
        /// 获取成果列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<PagedListDto<AchievementEntity>> GetAllAsync([FromQuery] PagedListRequestDto request)
        {
            return await achievementService.GetAllAsync(request);
        }

        /// <summary>
        /// 获取单个成果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AchievementEntity> GetAllAsync(int id)
        {
            return await achievementService.GetAsync(id);
        }

        /// <summary>
        /// 插入新的成果
        /// </summary>
        /// <param name="achievement"></param>
        /// <returns></returns>
        [HttpPost("Insert")]
        public async Task<int> InsertAsync(AchievementEntity achievement)
        {
            bool requireFile = configuration.GetValue<bool>("RequireFile");
            if (requireFile && string.IsNullOrEmpty(achievement.FileID))
            {
                throw new StatusBasedException("必须上传附件", System.Net.HttpStatusCode.BadRequest);
            }
            return await achievementService.InsertAsync(achievement);
        }

        /// <summary>
        /// 更新成果
        /// </summary>
        /// <param name="achievement"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(AchievementEntity achievement)
        {
            await achievementService.UpdateAsync(achievement);
            return Ok();
        }

        /// <summary>
        /// 删除成果
        /// </summary>
        /// <param name="id">成果ID</param>
        /// <returns></returns>
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await achievementService.DeleteAsync(id);
            return Ok();
        }
    }
}
