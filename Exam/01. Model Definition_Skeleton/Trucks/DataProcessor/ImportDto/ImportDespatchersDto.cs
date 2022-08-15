using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatchersDto
    {
        [XmlElement("Name")]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        public ImportDespatcherTrucksDto[] Trucks { get; set; }
    }
}
