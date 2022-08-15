using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trucks.Data.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; }

        [Required]
        [MaxLength(255)]
        public string Type { get; set; }

        [Required]
        [InverseProperty(nameof(ClientTruck.Client))]
        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } 
            = new List<ClientTruck>();
    }
}
