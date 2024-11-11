using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
