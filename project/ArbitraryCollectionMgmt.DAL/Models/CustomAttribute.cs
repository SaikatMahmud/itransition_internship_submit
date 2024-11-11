using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    public class CustomAttribute
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        [ForeignKey("CollectionId")]
        public virtual Collection Collection { get; set; }
        public virtual ICollection<CustomValue> CustomValues { get; set; }
    }
}
