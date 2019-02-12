using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrResData.Entities
{
    public class Resource : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
