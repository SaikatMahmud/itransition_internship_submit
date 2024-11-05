using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.Hubs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    [UserAccess]
    public class TokenController : Controller
    {
        private readonly ApiTokenService apiTokenService;
        public TokenController(IBusinessService serviceAccess)
        {
            apiTokenService = serviceAccess.ApiTokenService;
        }

        [HttpGet]
        [Route("api-token")]
        public IActionResult Index()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tokens = apiTokenService.GetAll(t => t.UserId == userId);
            tokens = tokens.OrderByDescending(t => t.CreatedAt).ToList();
            return View(tokens);
        }

        [HttpPost]
        [Route("generate-api-token")]
        public JsonResult Create(string name)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tokens = apiTokenService.Create(userId, name);
            if(tokens == null)
            {
                return Json(new { success = false, msg = "Failed to generate token" });
            }
            return Json(new { success = true, msg = "Token generated" });
        }

        [HttpPut]
        [Route("revoke-api-token")]
        public JsonResult Revoke(int apiTokenId)
        {
            var response = apiTokenService.Revoke(apiTokenId);
            if (!response)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

    }
}
