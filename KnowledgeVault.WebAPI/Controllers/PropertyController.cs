using KnowledgeVault.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeVault.WebAPI.Controllers
{
    /// <summary>
    /// 成果属性控制器
    /// </summary>
    /// <param name="propertyService"></param>
    /// <param name="configuration"></param>
    [ApiController]
    [Route("Property")]
    public class PropertyController(PropertyService propertyService, IConfiguration configuration) : KnowledgeVaultControllerBase
    {
        private readonly PropertyService propertyService = propertyService;
        private readonly IConfiguration configuration = configuration;

        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        [HttpGet("FirstAuthor")]
        public async Task<IActionResult> GetFirstAuthorsAsync()
        {
            return Ok(await propertyService.GetFirstAuthorsAsync());
        }

        /// <summary>
        /// 获取所有老师
        /// </summary>
        /// <returns></returns>
        [HttpGet("Correspond")]
        public async Task<IActionResult> GetCorrespondingAuthorsAsync()
        {
            return Ok(await propertyService.GetCorrespondingAuthorsAsync());
        }

        /// <summary>
        /// 获取所有年份
        /// </summary>
        /// <returns></returns>
        [HttpGet("Years")]
        public async Task<IActionResult> GetYearsAsync()
        {
            return Ok(await propertyService.GetYearsAsync());
        }

        /// <summary>
        /// 获取所有领域
        /// </summary>
        /// <returns></returns>
        [HttpGet("Themes")]
        public async Task<IActionResult> GetThemesAsync()
        {
            return Ok(await propertyService.GetThemesAsync());
        }
    }
}
