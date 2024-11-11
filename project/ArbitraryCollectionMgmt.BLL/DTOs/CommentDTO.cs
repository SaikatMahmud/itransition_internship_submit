using ArbitraryCollectionMgmt.DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string? CommentText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ItemDTO Item { get; set; }
        public UserDTO User { get; set; }
    }
}
