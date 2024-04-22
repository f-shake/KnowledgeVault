using Microsoft.AspNetCore.Mvc;

namespace KnowledgeVault.WebAPI.Controllers
{
    public class KnowledgeVaultControllerBase : ControllerBase
    {
        protected int GetUser()
        {
            return HttpContext.Session.GetInt32("user") ?? throw new Exception("未登录");
        }
    }
}
