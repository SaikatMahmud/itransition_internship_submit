using Microsoft.AspNetCore.Mvc;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.BLL.ViewModels;
using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArbitraryCollectionMgmt.Controllers
{
    [UserAccess(SD.Role.Admin)]
    public class UserController : Controller
    {
        private UserService userService;
        public UserController(IBusinessService serviceAccess)
        {
            userService = serviceAccess.UserService;
        }
        [Route("admin/manage-user")]
        public IActionResult Index()
        {
            var data = userService.GetAll();
            return View(data);
        }

        [Route("user/block")]
        [HttpPut]
        public JsonResult BlockUsers(int[] Ids)
        {
            var res = userService.Block(Ids);
            if (res)
            {
                TempData["success"] = "Operation success";
                return Json(new { success = true });
            }
            TempData["error"] = "Operation failed";
            return Json(new { success = false});
        }


        [Route("user/make-admin")]
        [HttpPut]
        public JsonResult MakeAdmin(int[] Ids)
        {
            var res = userService.MakeAdmin(Ids);
            if (res)
            {
                TempData["success"] = "Operation success";
                return Json(new { success = true });
            }
            TempData["error"] = "Operation failed";
            return Json(new { success = false });
        }

        [Route("user/remove-admin")]
        [HttpPut]
        public JsonResult RemoveAdmin(int[] Ids)
        {
            var res = userService.RemoveAdmin(Ids);
            if (res)
            {
                TempData["success"] = "Operation success";
                return Json(new { success = true });
            }
            TempData["error"] = "Operation failed";
            return Json(new { success = false });
        }


        [Route("user/unblock")]
        [HttpPut]
        public JsonResult UnblockUsers(int[] Ids)
        {
            var res = userService.Unblock(Ids);
            if (res)
            {
                TempData["success"] = "Operation success";
                return Json(new { success = true });
            }
            TempData["error"] = "Operation failed";
            return Json(new { success = false });
        }

        [Route("user/delete")]
        [HttpPut]
        public JsonResult DeleteUsers(int[] Ids)
        {
            var res = userService.Delete(Ids);
            if (res)
            {
                TempData["success"] = "Operation success";
                return Json(new { success = true });
            }
            TempData["error"] = "Operation failed";
            return Json(new { success = false });
        }

        [AllowAnonymous]
        [Route("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
