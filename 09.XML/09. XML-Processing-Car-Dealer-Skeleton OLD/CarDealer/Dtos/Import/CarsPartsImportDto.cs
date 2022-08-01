using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("partId")]
    public class CarsPartsImportDto
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
