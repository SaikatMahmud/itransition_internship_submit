using ArbitraryCollectionMgmt.BLL.ServiceAccess;
using ArbitraryCollectionMgmt.BLL.Services;
using Microsoft.AspNetCore.Mvc;


namespace ArbitraryCollectionMgmt.Web.Controllers
{
    [ApiController]
    public class ApiCollectionController : ControllerBase
    {
        private CollectionService collectionService;
        private CategoryService categoryService;
        private UserService userService;
        private ApiTokenService apiTokenService;
        public ApiCollectionController(IBusinessService serviceAccess)
        {
            collectionService = serviceAccess.CollectionService;
            categoryService = serviceAccess.CategoryService;
            userService = serviceAccess.UserService;
            apiTokenService = serviceAccess.ApiTokenService;
        }


        [HttpGet]
        [Route("api/user-collection-data/{apiToken}")]
        public IActionResult MyCollection(string apiToken)
        {
            var token = apiTokenService.Get(t => t.TokenKey == apiToken && t.IsRevoked == false);
            if (token == null)
            {
                return Unauthorized();
            }
            var data = collectionService.GetUserCollectionData(token.UserId);
            return Ok(data);
        }

    }
}
