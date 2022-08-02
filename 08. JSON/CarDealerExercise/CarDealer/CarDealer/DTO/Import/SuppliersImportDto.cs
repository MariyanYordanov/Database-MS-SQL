using Newtonsoft.Json;

namespace CarDealer.DTO.Import
{
    [JsonObject("")]
    public class SuppliersImportDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("")]
        public bool IsImporter { get; set; }
    }
}
