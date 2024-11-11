using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Areas.User.Controllers
{
    [Area("User")]
    [UserAccess]
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

        #region User ############################################################################################################

        [HttpGet]
        [Route("item/my-item/collection/{collectionId}")]
        public IActionResult MyItemOfCollection(int collectionId)
        {
            var collection = collectionService.Get(c => c.CollectionId == collectionId);
            if (collection == null)
            {
                TempData["error"] = "Collection not found!";
                return Redirect("/collection/my-collection");
            }
            ViewBag.collection = collection;
            return View();
        }

        [HttpGet]
        [Route("item/my-item/get-list")]
        public JsonResult GetMyItemOfCollection(int collectionId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int totalCount = 0;
            int filteredCount = 0;
            List<ItemDTO> result;
            result = itemService.GetCustomized(i => i.OwnedBy == userId && i.CollectionId == collectionId, search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount);
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
        [Route("item/my-item/all-item")]
        public IActionResult MyAllItem(int collectionId)
        {
            return View();
        }

        [HttpGet]
        [Route("item/my-item/get-all-item")]
        public JsonResult GetMyAllItem(int draw, int start, int length, string search, string orderColumn, string orderDirection)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int totalCount = 0;
            int filteredCount = 0;
            List<ItemDTO> result;
            result = itemService.GetCustomized(i => i.OwnedBy == userId, search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount, "Collection");
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
        [Route("item/my-item/create")]
        public IActionResult CreateMyItem(int collectionId = 0)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var collection = collectionService.Get(c => c.CollectionId == collectionId, "CustomAttributes");
            if (collection == null || collection.UserId != userId)
            {
                TempData["error"] = "Collection not available!";
                return Redirect("/collection/my-collection");
            }
            ViewBag.collection = collection;
            return View();
        }

        [HttpPost]
        [Route("item/my-item/create")]
        public IActionResult CreateMyItem(ItemCustomAttributeValueDTO item, string selectedTagIds, string newCreatedTags)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
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
            var result = itemService.Create(userId, item, tagIds, providedTags);
            if (result)
            {
                TempData["success"] = "Item added successfully!";
                return Redirect($"/item/my-item/collection/{item.CollectionId}");
            }
            TempData["error"] = "Could not added. Server error!";
            return Redirect($"/item/my-item/collection/{item.CollectionId}");
        }

        [HttpGet]
        [Route("item/my-item/edit/{itemId}")]
        public IActionResult EditMyItem(int itemId)
        {
            var tags = itemTagService.GetAll(i => i.ItemId == itemId, "Tag");
            var item = itemService.ViewItem(itemId);
            if (item == null)
            {
                TempData["error"] = "Item not found or server error!";
                return Redirect("/collection/my-collection");
            }
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (item.OwnedBy != userId)
            {
                TempData["error"] = "Access denied! You are not owner of this item!";
                return Redirect("/collection/my-collection");
            }
            ViewBag.ItemTags = tags;
            return View(item);
        }

        [HttpPost]
        [Route("item/my-item/edit/{itemId}")]
        public IActionResult EditMyItem(ItemCustomAttributeValueDTO item, string selectedTagIds, string newCreatedTags)
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
                return Redirect($"/item/my-item/collection/{item.CollectionId}");
            }
            TempData["error"] = "Could not updated. Server error!";
            return Redirect($"/item/my-item/collection/{item.CollectionId}");
        }
        #endregion

        #region User, Admin ---Common ############################################################################################################

        [HttpDelete]
        [Route("item/delete/{itemId}")]
        public IActionResult Delete(int itemId)
        {
            var result = itemService.Delete(itemId);
            if (result)
            {
                return Json(new { success = true, msg = "Item deleted!" });
            }
            return Json(new { success = false, msg = "Internal server error!" });
        }

        #endregion
    }
}
