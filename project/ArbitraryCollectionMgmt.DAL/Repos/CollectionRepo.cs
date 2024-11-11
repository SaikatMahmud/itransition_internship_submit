using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class CollectionRepo : Repository<Collection>, ICollection
    {
        private readonly ApplicationDbContext _db;
        public CollectionRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public (IEnumerable<Collection>, int TotalCount, int FilteredCount) GetAllSortFilterPage(Expression<Func<Collection, bool>> filter, string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null)
        {
            IQueryable<Collection> query = _db.Collections;
            int totalCount = _db.Collections.Count();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = $"\"{searchQuery}*\"";
                query = query.Where(c => EF.Functions.Contains(c.Name, searchQuery));
            }
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            var propertyInfo = typeof(Collection).GetProperty(orderColumn ?? "dummy");
            if (propertyInfo != null)
            {
                var parameter = Expression.Parameter(typeof(Collection), "x");
                var property = Expression.Property(parameter, orderColumn);
                var lambda = Expression.Lambda<Func<Collection, object>>(Expression.Convert(property, typeof(object)), parameter);

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

        public (IEnumerable<Collection>, int TotalCount, int FilteredCount) GetAllSortFilterPage(string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null)
        {
            IQueryable<Collection> query = _db.Collections;
            int totalCount = _db.Collections.Count();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = $"\"{searchQuery}*\"";
                query = query.Where(c => EF.Functions.Contains(c.Name, searchQuery));
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            var propertyInfo = typeof(Collection).GetProperty(orderColumn ?? "dummy");
            if (propertyInfo != null)
            {
                var parameter = Expression.Parameter(typeof(Collection), "x");
                var property = Expression.Property(parameter, orderColumn);
                var lambda = Expression.Lambda<Func<Collection, object>>(Expression.Convert(property, typeof(object)), parameter);

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

        public IEnumerable<Collection> GetTopLargest(int count, string? includeProperties = null)
        {
            IQueryable<Collection> query = _db.Collections;
            query = query.OrderByDescending(c => c.Items.Count()).Take(count);
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
