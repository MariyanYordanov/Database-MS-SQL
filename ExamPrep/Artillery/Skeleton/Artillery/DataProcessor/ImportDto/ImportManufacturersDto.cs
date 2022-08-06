using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ImportManufacturersDto
    {
        [XmlElement(nameof(ManufacturerName))]
        [StringLength(40, MinimumLength = 4)]
        public string ManufacturerName { get; set; }

        [XmlElement(nameof(Founded))]
        [StringLength(100, MinimumLength = 10)]
        public string Founded { get; set; }
    }
}
