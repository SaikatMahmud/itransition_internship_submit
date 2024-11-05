using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Areas.User.Controllers
{
    [Area("User")]
    [UserAccess]
    public class CollectionController : Controller
    {
        private CollectionService collectionService;
        private CategoryService categoryService;
        private UserService userService;
        public CollectionController(IBusinessService serviceAccess)
        {
            collectionService = serviceAccess.CollectionService;
            categoryService = serviceAccess.CategoryService;
            userService = serviceAccess.UserService;
        }


        #region User ############################################################################################################

        [HttpGet]
        [Route("collection/my-collection")]
        public IActionResult MyCollection()
        {
            IEnumerable<SelectListItem> catList = categoryService.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString(),
            });
            ViewBag.catList = catList;
            return View();
        }

        [HttpGet]
        [Route("collection/get-my-collection")]
        public JsonResult GetMyCollection(int categoryId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int totalCount = 0;
            int filteredCount = 0;
            List<CollectionDTO> result;
            if (categoryId == 0)
            {
                result = collectionService.GetCustomized(c => c.UserId == userId, search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount, "Category");
            }
            else
            {
                result = collectionService.GetCustomized(c => c.UserId == userId && c.CategoryId == categoryId, search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount, "Category");
            }

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
        [Route("collection/my-collection/create")]
        public IActionResult CreateMyCollection()
        {
            IEnumerable<SelectListItem> catList = categoryService.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString(),
            });
            ViewBag.catList = catList;
            IEnumerable<SelectListItem> fieldList = typeof(SD.FieldType).GetFields().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.GetValue(null).ToString(),
            });
            ViewBag.fieldList = fieldList;
            return View();
        }

        [HttpPost]
        [Route("collection/my-collection/create")]
        public IActionResult CreateMyCollection(CollectionCustomAttributeDTO collection, IFormFile? collectionImage)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> catList = categoryService.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString(),
                });
                ViewBag.catList = catList;
                IEnumerable<SelectListItem> fieldList = typeof(SD.FieldType).GetFields().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.GetValue(null).ToString(),
                });
                ViewBag.fieldList = fieldList;
                return View(collection);
            }
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (collectionImage != null)
            {
                var imageUrl = ImageControlService.UploadCollectionImage(collectionImage);
                collection.ImageUrl = imageUrl;
            }
            var result = collectionService.Create(userId, collection);
            if (result)
            {
                TempData["success"] = "Collection created successfully!";
                return RedirectToAction("MyCollection");
            }
            else TempData["error"] = "Could not added. Server error!";
            return RedirectToAction("MyCollection");
        }

        [HttpGet]
        [Route("collection/my-collection/edit/{collectionId}")]
        public IActionResult EditMyCollection(int collectionId)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var collection = collectionService.Get(c => c.CollectionId == collectionId, "CustomAttributes");
            if (collection == null)
            {
                TempData["error"] = "Collection not found!";
                return RedirectToAction("MyCollection");
            }
            if (collection.UserId != userId)
            {
                TempData["error"] = "Access denied! Collection not belongs to you.";
                return RedirectToAction("MyCollection");
            }
            IEnumerable<SelectListItem> catList = categoryService.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString(),
            });
            ViewBag.catList = catList;
            IEnumerable<SelectListItem> fieldList = typeof(SD.FieldType).GetFields().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.GetValue(null).ToString(),
            });
            ViewBag.fieldList = fieldList;
            return View(collection);
        }

        [HttpPost]
        [Route("collection/my-collection/edit/{collectionId}")]
        public IActionResult EditMyCollection(CollectionCustomAttributeDTO collection, IFormFile? collectionImage, string? existingImageUrl)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> catList = categoryService.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryId.ToString(),
                });
                ViewBag.catList = catList;
                IEnumerable<SelectListItem> fieldList = typeof(SD.FieldType).GetFields().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.GetValue(null).ToString(),
                });
                ViewBag.fieldList = fieldList;
                return View(collection);
            }
            if (collectionImage != null)
            {
                var imageUrl = ImageControlService.UploadCollectionImage(collectionImage, existingImageUrl);
                collection.ImageUrl = imageUrl;
            }
            var result = collectionService.Update(collection);
            if (result)
            {
                TempData["success"] = "Collection updated successfully!";
                return RedirectToAction("MyCollection");
            }
            else TempData["error"] = "Could not updated. Server error!";
            return RedirectToAction("MyCollection");
        }
        #endregion


        #region User, Admin ---Common ############################################################################################################

        [HttpDelete]
        [Route("collection/delete/{collectionId}")]
        public IActionResult Delete(int collectionId)
        {
            var result = collectionService.Delete(collectionId);
            if (result)
            {
                return Json(new { success = true, msg = "Collection deleted!" });
            }
            return Json(new { success = false, msg = "Internal server error!" });
        }

        [HttpPut]
        [Route("collection/remove-image/{collectionId}")]
        public IActionResult RemoveImage(int collectionId)
        {
            var result = collectionService.RemoveImageUrl(collectionId);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpDelete]
        [Route("collection/attribute/delete/{attributeId}")]
        public IActionResult DeleteAttribute(int attributeId)
        {
            var result = collectionService.DeleteAttribute(attributeId);
            if (result)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        #endregion

    }
}
