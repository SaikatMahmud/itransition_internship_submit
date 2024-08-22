using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class ItemTagRepo : Repository<ItemTag>, IItemTag
    {
        private readonly ApplicationDbContext _db;
        public ItemTagRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
