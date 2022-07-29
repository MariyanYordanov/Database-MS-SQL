using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.Product
{
    [JsonObject]
    public class ImportProductDto
    {
        [Required]
        [MinLength(3)]
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [JsonProperty(nameof(SellerId))]
        public int SellerId { get; set; }

        [JsonProperty(nameof(BuyerId))]
        public int? BuyerId { get; set; }
    }
}
