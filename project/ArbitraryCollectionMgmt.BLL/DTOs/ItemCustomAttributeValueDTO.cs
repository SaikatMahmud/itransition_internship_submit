using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class ItemCustomAttributeValueDTO : ItemDTO
    {
        [ValidateNever]
        public List<CustomValueDTO> CustomValues { get; set; }
        //[ValidateNever]
       // public List<CustomAttributeDTO> CustomAttributes { get; set; }
    }
}
