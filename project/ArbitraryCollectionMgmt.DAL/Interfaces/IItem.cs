using ArbitraryCollectionMgmt.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Interfaces
{
    public interface IItem : IRepo<Item>
    {
        (IEnumerable<Item>, int TotalCount, int FilteredCount) GetAllSortFilterPage(Expression<Func<Item, bool>> filter, string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null);
        (IEnumerable<Item>, int TotalCount, int FilteredCount) GetAllSortFilterPage(string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null);

        IEnumerable<Item> GetRecentlyAdded(int count, string? includeProperties = null);
    }
}
