using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountriesDto
    {
        [XmlElement(nameof(CountryName))]
        [StringLength(60, MinimumLength = 4)]
        public string CountryName { get; set; }

        [XmlElement(nameof(ArmySize))]
        [Range(50_000, 10_000_000)]
        public int ArmySize { get; set; }
    }
}
