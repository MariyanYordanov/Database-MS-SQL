using Newtonsoft.Json;

namespace ProductShop.DTOs.Category
{
    [JsonObject]
    public class ExportAllCaregory
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public decimal AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}
