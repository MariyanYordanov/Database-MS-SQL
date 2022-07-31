using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType(TypeName ="Supplier")]
    public class SupplierDto
    {
        [XmlElement(ElementName ="name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "isImporter")]
        public bool IsImporter { get; set; }
    }
}
