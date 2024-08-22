using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    [UserAccess]
    public class LikeController : Controller
    {
        private LikeService likeService;

        public LikeController(IBusinessService serviceAccess)
        {
            likeService = serviceAccess.LikeService;
        }
        [HttpGet]
        public JsonResult CheckUserLike(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = likeService.CheckUserLike(userId, id);
            return Json( new { success = true, IsLiked = result });
        }

        [HttpPost]
        public JsonResult AddLike(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = likeService.AddItemLike(userId, id);
            return Json(new { success = result });
        }

        [HttpDelete]
        public JsonResult RemoveLike(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = likeService.RemoveItemLike(userId, id);
            return Json(new { success = result });
        }
    }
}
