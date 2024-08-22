using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.Hubs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    [UserAccess]
    public class CommentController : Controller
    {
        private readonly CommentService commentService;
        private readonly UserService userService;
        private readonly IHubContext<CommentHub, ICommentHub> commentHub;
        public CommentController(IBusinessService serviceAccess, IHubContext<CommentHub, ICommentHub> _commentHub)
        {
            commentService = serviceAccess.CommentService;
            userService = serviceAccess.UserService;
            commentHub = _commentHub;
        }
        [HttpPost]
        public JsonResult AddComment(int itemId, string commentText)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = commentService.AddComment(userId, itemId, commentText);
            if(result == null) return Json(new { success = false });
            var user = userService.Get(u => u.UserId == userId);
            result.User = user;
            _ = commentHub.Clients.Group(result.ItemId.ToString()).ReceiveComment(result);
            return Json(new { success = true });
        }
        [HttpGet]
        public JsonResult GetAllComments(int id)
        {
            var result = commentService.GetComments(id, "User");
            return Json(new { success = true, data = result });
        }
    }
}

