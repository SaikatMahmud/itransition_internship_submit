using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class CollectionDTO
    {
        public int CollectionId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CategoryDTO? Category { get; set; }
        public UserDTO? User { get; set; }
    }
}
