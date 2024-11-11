using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsAdmin { get; set; }
        public string? UserStatus { get; set; }
    }
}
