using KnowledgeVault.WebAPI.Entity;
using KnowledgeVault.WebAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KnowledgeVault.WebAPI.Controllers
{
//    [ApiController]
//    [Route("[controller]")]
//    public class UserController(UserService userService) : KnowledgeVaultControllerBase
//    {
//        private readonly UserService userService = userService;

//        [HttpPost("Login")]
//        public async Task<IActionResult> LoginAsync(AchievementEntity user)
//        {
//#if DEBUG
//            var users = await userService.GetUsersAsync();
//            if (users.Count == 0)
//            {
//                await userService.AddUserAsync("string", "string", "string");
//            }
//#endif
//            var dbUser = await userService.GetUserAsync(user.Username);
//            if (dbUser == null)
//            {
//                return Unauthorized("用户不存在");
//            }
//            if (dbUser.Password != user.Password)
//            {
//                return Unauthorized("用户名和密码不匹配");
//            }
//            if (user.GroupName != dbUser.GroupName)
//            {
//                await userService.UpdateGroupNameAsync(dbUser.Id, user.GroupName);
//            }
//            HttpContext.Session.SetInt32("user", dbUser.Id);
//            return Ok();
//        }
//    }
}
