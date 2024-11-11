//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Models;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Password { internal get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
