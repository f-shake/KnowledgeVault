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
    [ApiController]
    [Route("[controller]")]
    public class UserController(IConfiguration config) : KnowledgeVaultControllerBase
    {
        private readonly IConfiguration config = config;

        /// <summary>
        /// ≤‚ ‘π‹¿Ì‘±Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("{token}")]
        public async Task<bool> CheckTokenAsync(string token)
        {
            var realToken = config["AppConfiguration:Token"];

            return realToken == token;
        }
    }
}
