namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Town
    {
        public Town()
        {
            this.Teams = new HashSet<Team>();
        }

        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength(GlobalConastants.TownNamerMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [InverseProperty(nameof(Team.Town))]
        public ICollection<Team> Teams { get; set; }
    }
}
