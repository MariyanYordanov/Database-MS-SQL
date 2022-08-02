using Newtonsoft.Json;

namespace CarDealer.DTO.Import
{
    [JsonObject]
    public class SalesImportDto
    {
        [JsonProperty("carId")]
        public int CarId { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }

        [JsonProperty("discount")]
        public int Discount { get; set; }
    }

}
