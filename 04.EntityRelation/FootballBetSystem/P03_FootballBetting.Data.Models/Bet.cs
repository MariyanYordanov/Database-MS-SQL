namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Bet
    {
        [Key]
        public int BetId { get; set; }

        [Required]
        [MaxLength()]
        public decimal Amount { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public Prediction Prediction { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
