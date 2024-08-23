using ArbitraryCollectionMgmt.DAL.Models;

namespace ArbitraryCollectionMgmt.DAL.Interfaces
{
    public interface ISearch
    {
        SearchResult GetSearchResult(string searchQuery);
        SearchResult GetItemWithMatchedTag(int tagId);

    }
}
