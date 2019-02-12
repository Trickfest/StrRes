using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrResData.Entities
{
    public class Tenant : BaseEntity
    {
        public long TenantId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string AccessToken { get; set; }
        
        public List<Resource> Resources { get; set; }       
    }
}
