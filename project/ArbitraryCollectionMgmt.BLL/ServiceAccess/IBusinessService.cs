using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.BLL.Services;

namespace ArbitraryCollectionMgmt.BLL.ServiceAccess
{
    public interface IBusinessService
    {
        public UserService UserService { get; set; }
        public AuthService AuthService { get; set; }
        public CollectionService CollectionService { get; set; }
        public ItemService ItemService { get; set; }
        public CategoryService CategoryService { get; set; }
        public TagService TagService { get; set; }
        public ItemTagService ItemTagService { get; set; }
        public LikeService LikeService { get; set; }
        public CommentService CommentService { get; set; }
        public SearchService SearchService { get; set; }
        public HomepageService HomepageService { get; set; }
        public ApiTokenService ApiTokenService { get; set; }
    }
}
