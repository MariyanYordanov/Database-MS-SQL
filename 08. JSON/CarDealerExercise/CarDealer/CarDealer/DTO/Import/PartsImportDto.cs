using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTO.Import
{
    [JsonObject("")]
    public class PartsImportDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }    

        [JsonProperty("supplierId")]
        [Required]
        public int SupplierId { get; set; }
    }
}
