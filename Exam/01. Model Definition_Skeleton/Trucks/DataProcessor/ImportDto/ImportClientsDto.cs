using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Trucks.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportClientsDto
    {
        [JsonProperty("Name")]
        [MaxLength(40)]
        [MinLength(3)]
        public string Name { get; set; }

        [JsonProperty("Nationality")]
        [MaxLength(40)]
        [MinLength(2)]
        public string Nationality { get; set; }

        [JsonProperty("Type")]
        [MaxLength(255)]
        public string Type { get; set; }

        [JsonProperty("Trucks")]
        public int[] Trucks { get; set; }
    }
}
