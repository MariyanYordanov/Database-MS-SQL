using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public class FullDto
    {
        [XmlElement("count")]
        public int Count { get; set; } 

        [XmlArray("users")]
        public UsersWithProductsDto[] Users { get; set; }
    }
}
