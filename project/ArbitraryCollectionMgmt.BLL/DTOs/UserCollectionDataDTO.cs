using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class UserCollectionDataDTO
    {
        public string Owner { get; set; }
        public List<CollectionData> Collections { get; set; }

    }

    public class CollectionData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ItemCount { get; set; }
        public string Category { get; set; }
    }
}
