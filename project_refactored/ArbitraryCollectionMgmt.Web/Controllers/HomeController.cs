using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HomepageService homepageService;
        public HomeController(IBusinessService serviceAccess)
        {
            homepageService = serviceAccess.HomepageService;
        }

        public IActionResult Index()
        {
            var result = homepageService.GetHomepageData(7, 5);
            return View(result);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
