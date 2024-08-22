using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    public class SearchController : Controller
    {
        private SearchService searchService;
        public SearchController(IBusinessService serviceAccess)
        {
            searchService = serviceAccess.SearchService;
        }
        public IActionResult SearchResult(string searchInArbitaryCollection = "")
        {
            if (string.IsNullOrEmpty(searchInArbitaryCollection))
            {
                TempData["error"] = "Please enter search keywords!";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            var result = searchService.GetSearchResult(searchInArbitaryCollection);
            ViewBag.SearchQuery = searchInArbitaryCollection;
            return View(result);
        }
    }
}
