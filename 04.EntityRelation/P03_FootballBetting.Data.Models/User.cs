namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(GlobalConastants.UsernameMaxLength)]
        public string Username { get; set; }

        [Required]
        [MaxLength(GlobalConastants.UserPasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(GlobalConastants.UserEmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(GlobalConastants.UserNameMaxLength)]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        [InverseProperty(nameof(Bet.User))]
        public virtual ICollection<Bet> Bets { get; set; }
    }
}
