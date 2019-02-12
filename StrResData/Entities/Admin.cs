using System.ComponentModel.DataAnnotations;

namespace StrResData.Entities
{
    public class Admin : BaseEntity
    {
        [Key]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string AccessToken { get; set; }
    }
}