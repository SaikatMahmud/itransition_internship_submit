﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string? CommentText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}