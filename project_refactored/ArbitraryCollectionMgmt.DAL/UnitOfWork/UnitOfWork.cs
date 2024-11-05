using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using ArbitraryCollectionMgmt.DAL.Repos;

namespace ArbitraryCollectionMgmt.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ICategory Category { get; private set; }
        public ICollection Collection { get; private set; }
        public ICustomAttribute CustomAttribute { get; private set; }
        public IItem Item { get; private set; }
        public ICustomValue CustomValue { get; private set; }
        public IItemTag ItemTag { get; private set; }
        public ITag Tag { get; private set; }
        public IUser User { get; private set; }
        public IUserLogin UserLogin { get; private set; }
        public ILike Like { get; private set; }
        public IComment Comment { get; private set; }
        public ISearch Search { get; private set; }
        public IApiToken ApiToken { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepo(_db);
            Collection = new CollectionRepo(_db);
            CustomAttribute = new CustomAttributeRepo(_db);
            Item = new ItemRepo(_db);
            CustomValue = new CustomValueRepo(_db);
            ItemTag = new ItemTagRepo(_db);
            Tag = new TagRepo(_db);
            User = new UserRepo(_db);
            UserLogin = new UserLoginRepo(_db);
            Like = new LikeRepo(_db);
            Comment = new CommentRepo(_db);
            Search = new SearchRepo(_db);
            ApiToken = new ApiTokenRepo(_db);
        }
    }
}
