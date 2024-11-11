using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class ItemRepo : Repository<Item>, IItem
    {
        private readonly ApplicationDbContext _db;
        public ItemRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public (IEnumerable<Item>, int TotalCount, int FilteredCount) GetAllSortFilterPage(Expression<Func<Item, bool>> filter, string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null)
        {
            IQueryable<Item> query = _db.Items;
            int totalCount = _db.Items.Count();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = $"\"{searchQuery}*\"";
                query = query.Where(i => EF.Functions.Contains(i.Name, searchQuery));
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            var propertyInfo = typeof(Item).GetProperty(orderColumn ?? "dummy");
            if (propertyInfo != null)
            {
                var parameter = Expression.Parameter(typeof(Item), "x");
                var property = Expression.Property(parameter, orderColumn);
                var lambda = Expression.Lambda<Func<Item, object>>(Expression.Convert(property, typeof(object)), parameter);

                if (orderDirection == "asc")
                {
                    query = query.OrderBy(lambda);
                }
                else
                {
                    query = query.OrderByDescending(lambda);
                }
            }
            int filterCount = query.Count();
            query = query.Skip(skip).Take(take);
            return (query.AsNoTracking().ToList(), totalCount, filterCount);
        }

        public (IEnumerable<Item>, int TotalCount, int FilteredCount) GetAllSortFilterPage(string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null)
        {
            IQueryable<Item> query = _db.Items;
            int totalCount = _db.Items.Count();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = $"\"{searchQuery}*\"";
                query = query.Where(i => EF.Functions.Contains(i.Name, searchQuery));
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            var propertyInfo = typeof(Item).GetProperty(orderColumn ?? "dummy");
            if (propertyInfo != null)
            {
                var parameter = Expression.Parameter(typeof(Item), "x");
                var property = Expression.Property(parameter, orderColumn);
                var lambda = Expression.Lambda<Func<Item, object>>(Expression.Convert(property, typeof(object)), parameter);

                if (orderDirection == "asc")
                {
                    query = query.OrderBy(lambda);
                }
                else
                {
                    query = query.OrderByDescending(lambda);
                }
            }
            int filteredCount = query.Count();
            query = query.Skip(skip).Take(take);
            return (query.AsNoTracking().ToList(), totalCount, filteredCount);
        }

        public IEnumerable<Item> GetRecentlyAdded(int count, string? includeProperties = null)
        {
            IQueryable<Item> query = _db.Items;
            query = query.OrderByDescending(i => i.CreatedAt).Take(count);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.AsNoTracking().ToList();
        }
    }
}
