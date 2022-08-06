using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string ManufacturerName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Founded { get; set; }

        [InverseProperty(nameof(Gun.Manufacturer))]
        public virtual List<Gun> Guns { get; set; } = new List<Gun>();
    }

    /*
     Manufacturer
        Id – integer, Primary Key
        ManufacturerName – unique text with length[4…40] (required)
        Founded – text with length[10…100] (required)
        Guns – a collection of Gun
     */
}
