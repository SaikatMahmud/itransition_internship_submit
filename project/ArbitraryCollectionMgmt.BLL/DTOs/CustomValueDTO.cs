using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class CustomValueDTO
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int CustomAttributeId { get; set; }
        public string FieldValue { get; set; }
        public ItemDTO Item { get; set; } //added while creating Search result View page
        public CustomAttributeDTO CustomAttribute { get; set; }
    }
}
