using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserMgmt.Auth;
using UserMgmt.BLL.ServiceAccess;
using UserMgmt.BLL.Services;
using UserMgmt.Models;

namespace UserMgmt.Controllers
{
    [UserAccess]
    public class HomeController : Controller
    {
        private UserService userService;
        public HomeController(IBusinessService serviceAccess)
        {
            userService = serviceAccess.UserService;
        }

        public IActionResult Index()
        {
            var data = userService.GetAll("UserLogin");
            return View(data);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
