using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Theatre.DataProcessor.ImportDto
{
    public class TheatersTicketsDto
    {
        [JsonProperty("Name")]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }

        [JsonProperty("NumberOfHalls")]
        [Required]
        [Range(typeof(sbyte), "1", "10", ConvertValueInInvariantCulture = true, ParseLimitsInInvariantCulture = true)]
        public sbyte NumberOfHalls { get; set; }

        [JsonProperty("Director")]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Director { get; set; }

        [JsonProperty("Tickets")]
        [Required]
        public List<TicketsDto> Tickets { get; set; }
    }

}
