using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArbitraryCollectionMgmt.Web.Controllers
{
    public class TagController : Controller
    {
        private TagService tagService;
        private ItemTagService itemTagService;
        public TagController(IBusinessService serviceAccess)
        {
            tagService = serviceAccess.TagService;
            itemTagService = serviceAccess.ItemTagService;
        }

        [UserAccess]
        [HttpGet]
        public JsonResult GetMatch(string search)
        {
            var tag = tagService.GetMatchedTags(search);
            //return Json(new { data = tag });
            return Json(tag);
        }

        [UserAccess]
        [HttpDelete]
        [Route("ItemTag/Delete/{id}")]
        public JsonResult DeleteItemTag(int id)
        {
            var result = itemTagService.DeleteItemTag(id);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        [Route("tag/get-all")]
        public JsonResult GetAllTag()
        {
            var result = tagService.GetAll();
            return Json(new { success = true, data = result});
        }



    }
}
