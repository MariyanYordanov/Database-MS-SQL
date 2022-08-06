using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string CountryName { get; set; }

        [Required]
        [Range(50_000, 10_000_000)]
        public int ArmySize { get; set; }

        [InverseProperty(nameof(CountryGun.Country))]
        public virtual List<CountryGun> CountriesGuns { get; set; } = new List<CountryGun>();
    }
    /*
        Id – integer, Primary Key
        CountryName – text with length[4, 60] (required)
        ArmySize – integer in the range[50_000….10_000_000] (required)
        CountriesGuns – a collection of CountryGun
        
    */
}
