using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public string? UserStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        public virtual ICollection<Collection> Collection { get; set; }
        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
