using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TeisterMask.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportEmployeesDto
    {
        [JsonProperty("Username")]
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        [MaxLength(40)]
        [MinLength(3)]
        public string Username { get; set; }

        [JsonProperty("Email"), Required, EmailAddress, MaxLength(250)]
        public string Email { get; set; }

        [JsonProperty("Phone")]
        [Phone]
        [RegularExpression(@"^(\d{3}-\d{3}-\d{4})$")]
        [MaxLength(12)]
        public string Phone { get; set; }

        [JsonProperty("Tasks")]
        public int[] Tasks { get; set; }
    }
}
