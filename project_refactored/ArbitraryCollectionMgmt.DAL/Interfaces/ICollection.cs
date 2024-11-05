using ArbitraryCollectionMgmt.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Interfaces
{
    public interface ICollection : IRepo<Collection>
    {
        (IEnumerable<Collection>, int TotalCount, int FilteredCount) GetAllSortFilterPage(Expression<Func<Collection, bool>> filter, string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null);
        (IEnumerable<Collection>, int TotalCount, int FilteredCount) GetAllSortFilterPage(string searchQuery, int skip, int take, string orderColumn, string orderDirection, string? includeProperties = null);

        IEnumerable<Collection> GetTopLargest(int count, string? includeProperties = null);
    }
}
