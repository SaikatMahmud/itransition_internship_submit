using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class SearchRepo : ISearch
    {
        private readonly ApplicationDbContext _db;
        public SearchRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public SearchResult GetSearchResult(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery)) return null;
            var userSearchKeywords = searchQuery.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var searchKeywords = userSearchKeywords.Where(s => s.Any(c => char.IsLetterOrDigit(c))).ToArray();
            SearchResult searchResult = new SearchResult();
            if (searchKeywords.Length == 1)
            {
                searchResult.Items = _db.Items.Where(i => EF.Functions.Contains(i.Name, searchKeywords[0])).ToList();
                searchResult.Comments = _db.Comments.Where(c => EF.Functions.Contains(c.CommentText, searchKeywords[0])).Include(c => c.Item).ToList();
                searchResult.CustomValues = _db.CustomValues.Where(cv => EF.Functions.Contains(cv.FieldValue, searchKeywords[0])).Include(cv => cv.Item).ToList();
                searchResult.Collections = _db.Collections.Where(c => EF.Functions.Contains(c.Name, searchKeywords[0])).ToList();
                // var tags = _db.Tags.Where(t => EF.Functions.Contains(t.Name, searchKeywords[0])).Include(t => t.) .ToList();
            }
            else
            {
                var joinedKeywords = string.Join(",", searchKeywords);
                searchResult.Items = _db.Items.Where(i => EF.Functions.Contains(i.Name, $"NEAR(({joinedKeywords}), 2, true)")).ToList();
                searchResult.Comments = _db.Comments.Where(c => EF.Functions.Contains(c.CommentText, $"NEAR(({joinedKeywords}), 2, true)")).Include(c => c.Item).ToList();
                searchResult.CustomValues = _db.CustomValues.Where(cv => EF.Functions.Contains(cv.FieldValue, $"NEAR(({joinedKeywords}), 2, true)")).Include(cv => cv.Item).ToList();
                searchResult.Collections = _db.Collections.Where(c => EF.Functions.Contains(c.Name, $"NEAR(({joinedKeywords}), 2, true)")).ToList();
            }
            return searchResult;
        }

        public SearchResult GetItemWithMatchedTag(int tagId)
        {
            var items = _db.Items.Where(i => i.ItemTags.Any(it => it.TagId == tagId)).ToList();
            return new SearchResult()
            {
                Items = items,
            };
        }
    }
}
