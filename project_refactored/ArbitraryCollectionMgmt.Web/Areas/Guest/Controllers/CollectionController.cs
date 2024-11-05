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

namespace ArbitraryCollectionMgmt.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
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
        [Route("collection/all-collection")]
        public IActionResult AllCollection_Guest()
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
        [Route("collection/get-all-collection")]
        public JsonResult GetAllCollection_Guest(int categoryId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
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
        [Route("collection/user/{userId}")]
        public IActionResult CollectionOfUser_Guest(int userId)
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
                return RedirectToAction("AllCollection_Guest");
            }
            ViewBag.catList = catList;
            ViewBag.userName = user.Name;
            return View();
        }

        [HttpGet]
        [Route("collection/get-user-collection/{userId}")]
        public JsonResult GetCollectionOfUser_Guest(int userId, [FromQuery] int categoryId, int draw, int start, int length, string search, string orderColumn, string orderDirection)
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
    }
}
