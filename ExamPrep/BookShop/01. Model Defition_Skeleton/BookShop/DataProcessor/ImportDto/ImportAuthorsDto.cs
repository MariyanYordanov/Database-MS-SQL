using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BookShop.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportAuthorsDto
    {
        [JsonProperty("FirstName")]
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [JsonProperty("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [JsonProperty("Phone")]
        [Required]
        [RegularExpression(@"^(\d{3})-(\d{3})-(\d{4})$")]
        [MaxLength(12)]
        [MinLength(12)]
        public string Phone { get; set; }

        [JsonProperty("Books")]
        public ImportAuthorBooksDto[] Books { get; set; }
    }
}
