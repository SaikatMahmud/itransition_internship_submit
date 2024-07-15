using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgmt.DAL.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
