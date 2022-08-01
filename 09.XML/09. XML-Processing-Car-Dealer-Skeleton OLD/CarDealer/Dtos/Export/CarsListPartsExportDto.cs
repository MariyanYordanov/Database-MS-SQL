using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("car")]
    public class CarsListPartsExportDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TraveledDistance{ get; set; }

        [XmlArray("parts")]
        public CarListPartsNestedExportDto[] PartsList { get; set; }
    }
}
