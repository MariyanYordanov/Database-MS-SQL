using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Trucks")]
    public class ImportDespatcherTrucksDto
    {
        [XmlElement("Truck")]
        [StringLength(8)]
        [RegularExpression(@"^[A-Z]{2}[\d]{4}[A-Z]{2}$")]
        public string RegistrationNumber { get; set; }

        [XmlElement("VinNumber")]
        [StringLength(17)]
        public string VinNumber { get; set; }

        [Required]
        [XmlElement("TankCapacity")]
        [Range(950, 1_420)]
        public int TankCapacity { get; set; }

        [Required]
        [XmlElement("CargoCapacity")]
        [Range(5_000, 29_000)]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        [Range(0, 3)]
        [EnumDataType(typeof(CategoryType))]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        [Range(0, 4)]
        [EnumDataType(typeof(MakeType))]
        public int MakeType { get; set; }
    }
}
