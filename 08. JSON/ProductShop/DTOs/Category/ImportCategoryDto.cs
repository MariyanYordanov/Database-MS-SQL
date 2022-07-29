using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.Category
{
    [JsonObject]
    public class ImportCategoryDto
    {
        [Required]
        [JsonProperty("name")]
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}
