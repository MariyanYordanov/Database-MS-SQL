using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BookShop.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportAuthorBooksDto
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }
    }
}
