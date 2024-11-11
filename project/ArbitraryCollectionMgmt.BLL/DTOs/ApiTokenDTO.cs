using ArbitraryCollectionMgmt.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class ApiTokenDTO
    {
        public int Id { get; set; }
        public string TokenKey { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO User { get; set; }
    }
}
