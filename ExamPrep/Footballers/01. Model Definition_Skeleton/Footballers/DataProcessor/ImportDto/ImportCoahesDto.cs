using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoahesDto
    {
        [XmlElement(nameof(Name))]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement(nameof(Nationality))]
        public string Nationality { get; set; }

        [XmlArray(nameof(Footballers))]
        public ImportCoachFootballersDto[] Footballers { get; set; }
    }
}
