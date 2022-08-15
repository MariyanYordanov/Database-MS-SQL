
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [StringLength(8)]
        [RegularExpression(@"^[A-Z]{2}[\d]{4}[A-Z]{2}$")]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(17)]
        public string VinNumber { get; set; }

        [Required]
        [Range(950, 1_420)]
        public int TankCapacity { get; set; }

        [Required]
        [Range(5_000, 29_000)]
        public int CargoCapacity { get; set; }

        [Required]
        [Range(0, 3)]
        [EnumDataType(typeof(CategoryType))]
        public CategoryType CategoryType { get; set; }

        [Required]
        [Range(0, 4)]
        [EnumDataType(typeof(MakeType))]
        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }
        public virtual Despatcher Despatcher { get; set; }

        [InverseProperty(nameof(ClientTruck.Truck))]
        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } 
            = new List<ClientTruck>();
    }
    
}
