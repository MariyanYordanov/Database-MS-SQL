using Newtonsoft.Json;

namespace CarDealer.DTO.Import
{
    [JsonObject]
    public class CarsImportDto
{
        [JsonProperty("make")]
        public string Make { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("travelledDistance")]
        public int TravelledDistance { get; set; }

        [JsonProperty("partsId")]
        public int[] PartsId { get; set; }
    }

}
