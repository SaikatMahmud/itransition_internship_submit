using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class ItemDTO
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int? CollectionId { get; set; }
        [ValidateNever]
        public CollectionDTO Collection { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? OwnedBy { get; set; }
        public UserDTO? User { get; set; }
    }
}
