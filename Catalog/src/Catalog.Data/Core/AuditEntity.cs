using System;
using System.ComponentModel.DataAnnotations;
using Catalog.Common.Core;

namespace Catalog.Data.Core
{
    public abstract class AuditEntity : BaseEntity
    {

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string CreatedIp { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedDate { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(50)]
        public string ModifiedIp { get; set; }

    }
}
