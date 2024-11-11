using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class CustomValueRepo : Repository<CustomValue>, ICustomValue
    {
        private readonly ApplicationDbContext _db;
        public CustomValueRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
