using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Catalog.Data.Core;

namespace Catalog.Data.Entities
{
    public class Product : AuditEntity
    {
        [Required]
        [StringLength(100)]
        public  string Name { get; set; }

        public int MeasureId { get; set; }

    }
}
