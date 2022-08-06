using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    [JsonObject]
    public class DepartmentsCellsImportDto
    {
        [Required]
        [JsonProperty("Name")]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        [JsonProperty("Cells")]
        public CellsImportDto[] Cells { get; set; }
    }
}
