using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserAccess(SD.Role.Admin)]
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
        [Route("admin/item/collection/{collectionId}")]
        public ActionResult ItemOfCollection_Admin(int collectionId)
        {
            var collection = collectionService.Get(c => c.CollectionId == collectionId);
            if (collection == null)
            {
                TempData["error"] = "Collection not found!";
                return Redirect("/admin/collection/all-collection");
            }
            ViewBag.collection = collection;
            return View();
        }

        [HttpGet]
        [Route("admin/item/collection/get-list")]
        public JsonResult GetItemOfCollection_Admin(int collectionId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
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
        [Route("admin/item/all-item")]
        public ActionResult AllItem_Admin()
        {
            return View();
        }

        [HttpGet]
        [Route("admin/item/get-all-item")]
        public JsonResult GetAllItem_Admin(int draw, int start, int length, string search, string orderColumn, string orderDirection)
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
        [Route("admin/item/create")]
        public IActionResult CreateItem_Admin(int collectionId = 0)
        {
            var collection = collectionService.Get(c => c.CollectionId == collectionId, "CustomAttributes");
            if (collection == null)
            {
                TempData["error"] = "Collection not available!";
                return Redirect("/admin/collection/all-collection");
            }
            ViewBag.collection = collection;
            ViewBag.ownerId = collection.UserId;
            return View();
        }

        [HttpPost]
        [Route("admin/item/create")]
        public IActionResult CreateItem_Admin(ItemCustomAttributeValueDTO item, string selectedTagIds, string newCreatedTags)
        {
            int[] tagIds = null;
            string[] providedTags = null;
            if (!string.IsNullOrEmpty(selectedTagIds))
            {
                tagIds = Array.ConvertAll(selectedTagIds.Split(","), int.Parse).Distinct().ToArray();
            }
            if (!string.IsNullOrEmpty(newCreatedTags))
            {
                providedTags = newCreatedTags.Split(",").Distinct().ToArray();
            }
            var result = itemService.Create(item.OwnedBy.Value, item, tagIds, providedTags);
            if (result)
            {
                TempData["success"] = "Item added successfully!";
                return Redirect($"/admin/item/collection/{item.CollectionId}");
            }
            TempData["error"] = "Could not added. Server error!";
            return Redirect($"/admin/item/collection/{item.CollectionId}");
        }

        [HttpGet]
        [Route("admin/item/edit/{itemId}")]
        public IActionResult EditItem_Admin(int itemId)
        {
            var tags = itemTagService.GetAll(i => i.ItemId == itemId, "Tag");
            var item = itemService.ViewItem(itemId);
            if (item == null)
            {
                TempData["error"] = "Item not found or server error!";
                return Redirect("admin/collection/all-collection");
            }

            ViewBag.ItemTags = tags;
            return View(item);
        }

        [HttpPost]
        [Route("admin/item/edit/{itemId}")]
        public IActionResult EditItem_Admin(ItemCustomAttributeValueDTO item, string selectedTagIds, string newCreatedTags)
        {
            int[] tagIds = null;
            string[] providedTags = null;
            if (!string.IsNullOrEmpty(selectedTagIds))
            {
                tagIds = Array.ConvertAll(selectedTagIds.Split(","), int.Parse).Distinct().ToArray();
            }
            if (!string.IsNullOrEmpty(newCreatedTags))
            {
                providedTags = newCreatedTags.Split(",").Distinct().ToArray();
            }
            var result = itemService.Update(item, tagIds, providedTags);
            if (result)
            {
                TempData["success"] = "Item updated successfully!";
                return Redirect($"/admin/item/collection/{item.CollectionId}");
            }
            TempData["error"] = "Could not updated. Server error!";
            return Redirect($"/admin/item/collection/{item.CollectionId}");
        }


    }
}
