using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTeamsDto
    {
        [JsonProperty("Name")]
        [StringLength(40, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z0-9\s.-]+$")]
        public string Name { get; set; }

        [JsonProperty("Nationality")]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; }

        [JsonProperty("Trophies")]
        [Range(typeof(int),"1", "2147483647")]
        public int Trophies { get; set; }

        [JsonProperty("Footballers")]
        public List<int> Footballers { get; set; }
    }
}
