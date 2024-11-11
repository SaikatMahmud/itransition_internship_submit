using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class CustomAttributeDTO
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string FieldType { get; set; }
        public CustomValueDTO CustomValue { get; set; }
        //[ValidateNever]
        // public CollectionDTO Collection { get; set; }
    }
}
