using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitraryCollectionMgmt.DAL.Models
{
    public class CustomValue
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int CustomAttributeId { get; set; }
        public string? FieldValue { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        [ForeignKey("CustomAttributeId")]
        public virtual CustomAttribute CustomAttribute { get; set; }
    }
}
