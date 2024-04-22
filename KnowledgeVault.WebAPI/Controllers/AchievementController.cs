using KnowledgeVault.Core.Dto;
using KnowledgeVault.Core.Entity;
using KnowledgeVault.Core.Service;
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
        /// 插入新的成果
        /// </summary>
        /// <param name="achievement"></param>
        /// <returns></returns>
        [HttpPost("Insert")]
        public async Task<int> InsertAsync(AchievementEntity achievement)
        {
            return await achievementService.InsertAsync(achievement);
        }

        /// <summary>
        /// 更新的成果
        /// </summary>
        /// <param name="achievement"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(AchievementEntity achievement)
        {
            await achievementService.UpdateAsync(achievement);
            return Ok();
        }
    }
}
