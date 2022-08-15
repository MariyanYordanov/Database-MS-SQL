using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        public string Position { get; set; }

        [InverseProperty(nameof(Truck.Despatcher))]
        public virtual ICollection<Truck> Trucks { get; set; } 
            = new List<Truck>(); 
    }
}
