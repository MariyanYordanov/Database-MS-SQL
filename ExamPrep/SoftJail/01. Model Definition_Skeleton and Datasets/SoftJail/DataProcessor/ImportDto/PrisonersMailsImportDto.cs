using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    [JsonObject]
    public class PrisonersMailsImportDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [JsonProperty("FullName")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^(The)\s([A-Z][a-z]*)$")]
        [JsonProperty("Nickname")]
        public string Nickname { get; set; }

        [Required]
        [Range(18, 65)]
        [JsonProperty("Age")]
        public int Age { get; set; }

        [Required]
        [JsonProperty("IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [JsonProperty("ReleaseDate")]
        public string ReleaseDate { get; set; }

        [JsonProperty("Bail")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        [JsonProperty("CellId")]
        public int? CellId { get; set; }

        [JsonProperty("Mails")]
        public MailImportDto[] Mails { get; set; }
    }
}
