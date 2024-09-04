using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.BLL.Services;
using ArbitraryCollectionMgmt.DAL.UnitOfWork;
using Microsoft.AspNetCore.SignalR;

namespace ArbitraryCollectionMgmt.BLL.ServiceAccess
{
    public class BusinessService : IBusinessService
    {
        public UserService UserService { get; set; }
        public AuthService AuthService { get; set; }
        public CollectionService CollectionService { get; set; }
        public ItemService ItemService { get; set; }
        public CategoryService CategoryService { get; set; }
        public TagService TagService { get; set; }
        public ItemTagService ItemTagService { get; set; }
        public CommentService CommentService { get; set; }
        public LikeService LikeService { get; set; }
        public SearchService SearchService { get; set; }
        public HomepageService HomepageService { get; set; }
        public ApiTokenService ApiTokenService { get; set; }


        public BusinessService(IUnitOfWork unitOfWork)
        {
            UserService = new UserService(unitOfWork);
            AuthService = new AuthService(unitOfWork);
            CollectionService =  new CollectionService(unitOfWork);
            CategoryService = new CategoryService(unitOfWork);
            ItemService = new ItemService(unitOfWork);
            TagService = new TagService(unitOfWork);
            ItemTagService = new ItemTagService(unitOfWork);
            CommentService = new CommentService(unitOfWork);
            LikeService = new LikeService(unitOfWork);
            SearchService = new SearchService(unitOfWork);
            HomepageService = new HomepageService(unitOfWork);
            ApiTokenService = new ApiTokenService(unitOfWork);
        }
    }
}
