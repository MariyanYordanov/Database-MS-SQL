namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Country
    {
        public Country()
        {
            this.Towns = new HashSet<Town>();
        }

        [Key]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(GlobalConastants.CountryNameMaxLength)]
        public string Name { get; set; }

        [InverseProperty(nameof(Town.Country))]
        public ICollection<Town> Towns { get; set; }
    }
}
