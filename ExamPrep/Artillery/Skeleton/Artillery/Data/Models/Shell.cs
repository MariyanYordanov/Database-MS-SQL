using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class Shell
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(2.0, 1_680.0)]
        public double ShellWeight { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Caliber { get; set; }

        [InverseProperty(nameof(Gun.Shell))]
        public virtual List<Gun> Guns { get; set; } = new List<Gun>();
    }

    /*  Id – integer, Primary Key
        ShellWeight – double in range  [2…1_680] (required)
        Caliber – text with length [4…30] (required)
        Guns – a collection of Gun
    */
}
