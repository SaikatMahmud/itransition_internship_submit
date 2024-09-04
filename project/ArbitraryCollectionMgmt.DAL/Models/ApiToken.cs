using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    [Index(nameof(TokenKey), IsUnique = true)]
    public class ApiToken
    {
        [Key]
        public int Id { get; set; }
        public string TokenKey { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
