namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using P03_FootballBetting.Data.Common;

    public class Team
    {
        public Team()
        {
            this.HomeGames = new HashSet<Game>();
            this.AwayGames = new HashSet<Game>();
            this.Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(GlobalConastants.TeamNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(GlobalConastants.TeamLogoUrlMaxLength)]
        public string LogoUrl { get; set; }

        [Required]
        [MaxLength(GlobalConastants.TeamInitialsMaxLength)]
        public string Initials { get; set; }

        // Regquired by default
        public decimal Budget { get; set; }

       [ForeignKey(nameof(PrimaryKitColor))]
        public int PrimaryKitColorId { get; set; }
        public virtual Color PrimaryKitColor { get; set; }

        [ForeignKey(nameof(SecondaryKitColor))]
        public int SecondaryKitColorId { get; set; }
        public virtual Color SecondaryKitColor { get; set; }

        [ForeignKey(nameof(TownId))]
        public int TownId { get; set; } // Foreign Key
        public virtual Town Town { get; set; } // Navigational property

        [InverseProperty(nameof(Game.HomeTeam))]
        public virtual ICollection<Game> HomeGames { get; set; }

        [InverseProperty(nameof(Game.AwayTeam))]
        public virtual ICollection<Game> AwayGames { get; set; }

        [InverseProperty(nameof(Player.Team))]
        public virtual ICollection<Player> Players { get; set; }
    }
}
