using Newtonsoft.Json;

namespace Artillery.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportGunsContriesDto
    {
        [JsonProperty(nameof(Id))]
        public int Id { get; set; }
    }
}
