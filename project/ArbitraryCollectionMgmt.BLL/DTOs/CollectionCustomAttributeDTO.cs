using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.BLL.DTOs
{
    public class CollectionCustomAttributeDTO : CollectionDTO
    {
        [ValidateNever]
        public List<CustomAttributeDTO> CustomAttributes { get; set; }
    }
}
