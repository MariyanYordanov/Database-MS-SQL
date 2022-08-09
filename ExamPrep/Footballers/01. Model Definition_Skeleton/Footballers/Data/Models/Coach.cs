using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    public class Coach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(450)]
        public string Nationality { get; set; }

        [InverseProperty(nameof(Footballer.Coach))]
        public virtual ICollection<Footballer> Footballers { get; set; } 
            = new List<Footballer>();
    }
}
