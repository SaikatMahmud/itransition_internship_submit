using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    public class SearchResult
    {
        public ICollection<Item> Items { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<CustomValue> CustomValues { get; set; }
        public ICollection<Collection> Collections { get; set; }
    }
}
