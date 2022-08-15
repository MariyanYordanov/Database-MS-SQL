using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatchers")]
    public class ExportDespatchersDto
    {
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }

        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; }

        [XmlArray("Trucks")]
        public ExportTrucksDto[] Trucks { get; set; }
    }
}
