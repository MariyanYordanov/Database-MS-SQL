using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class TicketsDto
    {
        [JsonProperty("Price")]
        [Required]
        [Range(typeof(decimal), "1.00", "10.00", ConvertValueInInvariantCulture = true, ParseLimitsInInvariantCulture = true)]
        public decimal Price { get; set; }

        [JsonProperty("RowNumber")]
        [Required]
        [Range(typeof(sbyte), "1", "10", ConvertValueInInvariantCulture = true, ParseLimitsInInvariantCulture = true)]
        public sbyte RowNumber { get; set; }

        [JsonProperty("PlayId")]
        [Required]
        public int PlayId { get; set; }

    }
}
