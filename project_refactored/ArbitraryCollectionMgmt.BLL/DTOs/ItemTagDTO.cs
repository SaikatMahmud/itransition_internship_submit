using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class ItemTagDTO
    {
        public int ItemTagId { get; set; }
        public int ItemId { get; set; }
        public int TagId { get; set; }
        public ItemDTO Item { get; set; }
        public TagDTO Tag { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
