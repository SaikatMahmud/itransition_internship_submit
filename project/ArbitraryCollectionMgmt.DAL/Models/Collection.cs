using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    public class Collection
    {
        [Key]
        public int CollectionId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<CustomAttribute> CustomAttributes { get; set; }
    }
}
