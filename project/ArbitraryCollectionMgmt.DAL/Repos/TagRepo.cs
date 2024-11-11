using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class TagRepo : Repository<Tag>, ITag
    {
        private readonly ApplicationDbContext _db;
        public TagRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public List<Tag> GetMatchedTags(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery)) return null;
            searchQuery = $"\"{searchQuery}*\"";
            var tags = _db.Tags.Where(t => EF.Functions.Contains(t.Name, searchQuery)).ToList();
            return tags;
        }
    }
}
