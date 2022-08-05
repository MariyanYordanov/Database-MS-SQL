using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class PeisonersExportDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("IncarcerationDate")]
        [Required]
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public MessageExportDto[] EncryptedMessages { get; set; }
    }
}
