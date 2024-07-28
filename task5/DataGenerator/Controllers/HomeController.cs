using DataGenerator.Models;
using DataGenerator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace DataGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            IEnumerable<SelectListItem> regionList = typeof(Region).GetFields().Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.GetValue(null).ToString(),
                Selected = r.GetValue(null).ToString() == Region.USA,
            });
            ViewBag.regionList = regionList;
            return View();
        }

        [HttpGet]
        public IActionResult GetUserData(int page = 1, string region = Region.USA, double error = 0, int seed = 1)
        {
            var users = UserGeneratorService.GetUsers(page, region, error, seed);

            return Json(new {data = users});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
