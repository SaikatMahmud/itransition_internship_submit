using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    public class SearchController : Controller
    {
        private SearchService searchService;
        public SearchController(IBusinessService serviceAccess)
        {
            searchService = serviceAccess.SearchService;
        }
        public IActionResult SearchResult([FromQuery] string searchQ = "", [FromQuery] string searchTag = "") //searchTag format "2_Tag2": id_tagname
        {
            if (!string.IsNullOrEmpty(searchTag))
            {
                Match match = Regex.Match(searchTag, @"^(\d+)_(.*)$");
                int tagId = int.TryParse(match.Groups[1].Value, out int temp) ? temp : 0;
                string tagName = match.Groups[2].Value;
                var tagSearchResult = searchService.GetSearchResultForTag(tagId);
                ViewBag.SearchTagName = tagName;
                return View(tagSearchResult);
            }

            if (string.IsNullOrEmpty(searchQ))
            {
                TempData["error"] = "Please enter search keywords!";
                //return Redirect(Request.Headers["Referer"].ToString());
                return Redirect(Request.Headers.Referer.ToString());
            }
            var result = searchService.GetSearchResult(searchQ);
            ViewBag.SearchQuery = searchQ;
            return View(result);
        }
    }
}
