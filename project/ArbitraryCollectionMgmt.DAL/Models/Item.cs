using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        [DefaultValue(0)]
        public int? CollectionId { get; set; }
        [ForeignKey("CollectionId")]
        public virtual Collection Collection { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? OwnedBy { get; set; }
        [ForeignKey("OwnedBy")]
        public virtual User User { get; set; }
        public virtual ICollection<ItemTag> ItemTags { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CustomValue> CustomValues { get; set;}
    }
}
