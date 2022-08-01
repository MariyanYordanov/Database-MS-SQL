using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("SoldProducts")]
    public class UsersWithProductsInnerDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public UsersWithProductsInnerNestedDto[] Products { get; set; }
    }
}
