using Microsoft.AspNetCore.Mvc;
using UserMgmt.BLL.ServiceAccess;
using UserMgmt.BLL.Services;
using UserMgmt.BLL.ViewModels;

namespace UserMgmt.Controllers
{
    public class UserController : Controller
    {
        private UserService userService;
        public UserController(IBusinessService serviceAccess)
        {
            userService = serviceAccess.UserService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAllUser()
        {
            var data = userService.GetAll("UserLogin");
            return Ok(data);
        }
        [Route("register")]
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [Route("register")]
        [HttpPost]
        public IActionResult Registration(UserRegistrationVM obj)
        {
            if (!ModelState.IsValid) return View(obj);
            string errorMsg;
            var result = userService.Create(obj, out errorMsg);
            if (result)
            {
                TempData["success"] = "Registration Success!";
                return RedirectToAction("Login","Auth");
            }
            TempData["error"] = errorMsg;
            return View(obj);
        }
        [Route("user/block")]
        [HttpPut]
        public IActionResult BlockUsers(int[] Ids)
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

        [Route("user/unblock")]
        [HttpPut]
        public IActionResult UnblockUsers(int[] Ids)
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
        public IActionResult DeleteUsers(int[] Ids)
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

    }
}
