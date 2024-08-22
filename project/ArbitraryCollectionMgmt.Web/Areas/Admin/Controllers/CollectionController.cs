using ArbitraryCollectionMgmt.Auth;
using ArbitraryCollectionMgmt.BLL.DTOs;
using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ArbitraryCollectionMgmt.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [UserAccess(SD.Role.Admin)]
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

        [HttpGet]
        [Route("admin/collection/all-collection")]
        public IActionResult AllCollection_Admin()
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
        [Route("admin/collection/get-all-collection")]
        public JsonResult GetAllCollection_Admin(int categoryId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<CollectionDTO> result;
            if (categoryId == 0)
            {
                result = collectionService.GetCustomized(search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount, "Category, User");
            }
            else
            {
                result = collectionService.GetCustomized(c => c.CategoryId == categoryId, search, start, length, orderColumn, orderDirection, out totalCount, out filteredCount, "Category, User");
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
        [Route("admin/collection/user/{userId}")]
        public IActionResult CollectionOfUser_Admin(int userId)
        {
            IEnumerable<SelectListItem> catList = categoryService.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString(),
            });
            var user = userService.Get(u => u.UserId == userId);
            if (user == null)
            {
                TempData["error"] = "User not found!";
                return RedirectToAction("AllCollection_Admin");
            }
            ViewBag.catList = catList;
            ViewBag.userName = user.Name;
            ViewBag.userId = userId;
            return View();
        }

        [HttpGet]
        [Route("admin/collection/get-user-collection/{userId}")]
        public JsonResult GetCollectionOfUser_Admin(int userId, [FromQuery] int categoryId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
        {
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
        [Route("admin/collection/create")]
        public IActionResult CreateCollection_Admin([FromQuery] int userId)
        {
            var user = userService.Get(u => u.UserId == userId);
            if (user == null)
            {
                TempData["error"] = "User not found!";
                return RedirectToAction("AllCollection_Admin");
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
            ViewBag.userId = userId;
            return View();
        }

        [HttpPost]
        [Route("admin/collection/create")]
        public IActionResult CreateCollection_Admin(CollectionCustomAttributeDTO collection, IFormFile? collectionImage)
        {
            int userId = collection.UserId;
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
                var imageUrl = ImageControlService.UploadCollectionImage(collectionImage);
                collection.ImageUrl = imageUrl;
            }
            var result = collectionService.Create(userId, collection);
            if (result)
            {
                TempData["success"] = "Collection created successfully!";
                return Redirect($"/admin/collection/user/{userId}");
            }
            else TempData["error"] = "Could not added. Server error!";
            return Redirect($"/admin/collection/user/{userId}");
        }

        [HttpGet]
        [Route("admin/collection/edit/{collectionId}")]
        public IActionResult EditCollection_Admin(int collectionId)
        {
            var collection = collectionService.Get(c => c.CollectionId == collectionId, "CustomAttributes");
            if (collection == null)
            {
                TempData["error"] = "Collection not found!";
                return RedirectToAction("AllCollection_Admin");
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
        [Route("admin/collection/edit/{collectionId}")]
        public IActionResult EditCollection_Admin(CollectionCustomAttributeDTO collection, IFormFile? collectionImage, string? existingImageUrl)
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
                return RedirectToAction("AllCollection_Admin");
            }
            else TempData["error"] = "Could not updated. Server error!";
            return RedirectToAction("AllCollection_Admin");
        }
    }
}
