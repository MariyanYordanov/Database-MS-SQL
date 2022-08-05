using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    [JsonObject]
    public class MailImportDto
    {
        [Required]
        [JsonProperty("Description")]
        public string Description { get; set; }

        [Required]
        [JsonProperty("Sender")]
        public string Sender { get; set; }

        [Required]
        [StringLength(250)]
        [RegularExpression(@"^[\d\w\s]*\sstr.$")]
        [JsonProperty("Address")]
        public string Address { get; set; }
    }
}
