using Newtonsoft.Json;
using System.Linq;

namespace ProductShop.DTOs.Product
{
    [JsonObject]
    public class ExportSoldProductsFullInfo
    {
        [JsonProperty("count")]
        public int ProductsCount => Products.Any() ? Products.Length : 0;

        [JsonProperty("products")]
        public ExportSoldProductShortInfoDto[] Products { get; set; }
    }
}
