using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class PrisonersImportDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
