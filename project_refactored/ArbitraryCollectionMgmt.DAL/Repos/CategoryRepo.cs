using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class CategoryRepo : Repository<Category>, ICategory
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
