using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class ItemController : Controller
    {
        private ItemService itemService;
        private ItemTagService itemTagService;
        private CollectionService collectionService;
        public ItemController(IBusinessService serviceAccess)
        {
            itemService = serviceAccess.ItemService;
            itemTagService = serviceAccess.ItemTagService;
            collectionService = serviceAccess.CollectionService;
        }
        [HttpGet]
        [Route("item/collection/{collectionId}")]
        public ActionResult ItemOfCollection_Guest(int collectionId)
        {
            var collection = collectionService.Get(c => c.CollectionId == collectionId);
            if (collection == null)
            {
                TempData["error"] = "Collection not found!";
                return Redirect("/collection/all-collection");
            }
            ViewBag.collection = collection;
            return View();
        }

        [HttpGet]
        [Route("item/collection/get-list/{collectionId}")]
        public JsonResult GetItemOfCollection_Guest(int collectionId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<ItemDTO> result;
            result = itemService.GetCustomized(i => i.CollectionId == collectionId, search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount);
            var response = new
            {
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = result
            };
            return Json(response);
        }

        [HttpGet]
        [Route("item/all-item")]
        public ActionResult AllItem_Guest()
        {
            return View();
        }

        [HttpGet]
        [Route("item/get-all-item")]
        public JsonResult GetAllItem_Guest(int draw, int start, int length, string search, string orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<ItemDTO> result;
            result = itemService.GetCustomized(search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount, "Collection, User");
            var response = new
            {
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = result
            };
            return Json(response);
        }

        [HttpGet]
        [Route("item/view/{itemId}")]
        public IActionResult ViewItem(int itemId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var item = itemService.ViewItem(itemId);
            if (item == null)
            {
                TempData["error"] = "Item not found or server error!";
                return Redirect("/collection/all-collection");
            }
            var tags = itemTagService.GetAll(i => i.ItemId == itemId, "Tag");
            var collection = collectionService.GetCollectionWithUser(c => c.CollectionId == item.CollectionId);
            ViewItemVM viewItemVM = new ViewItemVM();
            viewItemVM.Item = item;
            viewItemVM.Collection = collection;
            viewItemVM.ItemTags = tags;
            return View(viewItemVM);
        }
    }
}
