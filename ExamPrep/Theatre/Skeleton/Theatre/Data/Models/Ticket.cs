using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(typeof(decimal), "1.00", "10.00", ConvertValueInInvariantCulture = true, ParseLimitsInInvariantCulture = true)]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }
        public Play Play { get; set; }

        [Required]
        public int TheatreId { get; set; }
        public Theatre Theatre { get; set; }
    }
}
