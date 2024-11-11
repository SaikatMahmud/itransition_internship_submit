using ArbitraryCollectionMgmt.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class SearchResultDTO
    {
        public List<ItemDTO> Items { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public List<CustomValueDTO> CustomValues { get; set; }
        public List<CollectionDTO> Collections { get; set; }
    }
}
