using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [StringLength(250)]
        [RegularExpression(@"^[\d\w\s]* str.$")]
        public string Address { get; set; }

        [ForeignKey(nameof(Prisoner))]
        public int PrisonerId { get; set; }
        public Prisoner Prisoner { get; set; }
    }

    /*
     *  Id – integer, Primary Key
        Description– text (required)
        Sender – text (required)
        Address – text, consisting only of letters, spaces and numbers, which ends with “ str.” (required) (Example: “62 Muir   Hill  tr.“)
        PrisonerId - integer, foreign key (required)
        Prisoner – the mail's Prisoner (required)
     */
}
