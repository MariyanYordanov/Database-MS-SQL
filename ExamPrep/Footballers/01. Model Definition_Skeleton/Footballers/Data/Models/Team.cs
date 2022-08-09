using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s.-]+$")]
        public string Name { get; set; }

        [Required]
        [MaxLength(450)]
        public string Nationality { get; set; }

        [Required]
        public int Trophies { get; set; }

        [InverseProperty(nameof(TeamFootballer.Team))]
        public ICollection<TeamFootballer> TeamsFootballers { get; set; } 
            = new List<TeamFootballer>();
    }
}
