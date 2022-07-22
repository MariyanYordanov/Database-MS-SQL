namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        [Key]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(GlobalConastants.PositionNameMaxLength)]
        public string Name { get; set; }

        [InverseProperty(nameof(Player.Position))]
        public ICollection<Player> Players { get; set; }
    }
}
