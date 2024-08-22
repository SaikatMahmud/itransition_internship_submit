using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    [UserAccess]
    public class TagController : Controller
    {
        private TagService tagService;
        private ItemTagService itemTagService;
        public TagController(IBusinessService serviceAccess)
        {
            tagService = serviceAccess.TagService;
            itemTagService = serviceAccess.ItemTagService;
        }
        [HttpGet]
        public IActionResult GetMatch(string search)
        {
            var tag = tagService.GetMatchedTags(search);
            //return Json(new { data = tag });
            return Json(tag);
        }

        [HttpDelete]
        [Route("ItemTag/Delete/{id}")]
        public IActionResult DeleteItemTag(int id)
        {
            var result = itemTagService.DeleteItemTag(id);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
 
    }
}
